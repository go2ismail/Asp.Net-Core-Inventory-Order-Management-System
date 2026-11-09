using Microsoft.Playwright;
using NUnit.Framework;

namespace E2E;

/// <summary>
/// Shared CRUD flow for the list pages that follow the "Main Info" modal pattern:
/// toolbar Add/Edit/Delete (#AddCustom/#EditCustom/#DeleteCustom), a bootstrap modal
/// (#MainModal with #MainSaveButton) and SweetAlert success notifications.
/// </summary>
public abstract class CrudModuleTestBase : E2ETestBase
{
    /// <summary>Relative url of the list page, for example "/Customers/CustomerList".</summary>
    protected abstract string ListPath { get; }

    /// <summary>Document title rendered by the list page, for example "Customer List".</summary>
    protected abstract string ListTitle { get; }

    /// <summary>Prefix used to build unique entity names, for example "E2E-CUS".</summary>
    protected abstract string NamePrefix { get; }

    /// <summary>
    /// Some list pages (Customers, Vendors) render a second grid inside a contact modal,
    /// so rows are always resolved from the first grid on the page.
    /// </summary>
    protected ILocator MainGrid => Page.Locator(".e-grid").First;

    protected ILocator GridRows => MainGrid.Locator(".e-row");

    protected ILocator GridRow(string name) =>
        GridRows.Filter(new LocatorFilterOptions { HasText = name });

    protected string NewName() => $"{NamePrefix}-{Guid.NewGuid():N}";

    protected async Task OpenListAsync()
    {
        await Page.GotoAsync($"{BaseUrl}{ListPath}", new PageGotoOptions
        {
            WaitUntil = WaitUntilState.DOMContentLoaded
        });

        await Expect(Page).ToHaveTitleAsync(ListTitle);
        await Expect(Page.Locator("#AddCustom")).ToBeVisibleAsync();
        await Expect(GridRows.First).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions
        {
            Timeout = 20000
        });
    }

    /// <summary>
    /// Fills every field the module needs to pass its own client side validation.
    /// Implementations only touch the modal content, submitting is handled by the base class.
    /// </summary>
    protected abstract Task FillFormAsync(string name);

    protected async Task FillTextBoxAsync(string elementId, string? value)
    {
        var input = Page.Locator($"#{elementId}");
        await Expect(input).ToBeVisibleAsync();
        await input.FillAsync(value ?? string.Empty);
    }

    /// <summary>
    /// Picks an option from a Syncfusion EJ2 DropDownList. The control is located through the
    /// "for" attribute of its label, because EJ2 hides the placeholder element and inserts the
    /// real control (a ".e-input-group.e-ddl" wrapper) next to it. The selection is verified on the
    /// hidden select the widget keeps in sync, and retried because a click during the popup open
    /// animation is ignored by the widget.
    /// </summary>
    protected async Task SelectDropDownAsync(string fieldId, string optionText)
    {
        var field = Page.Locator($"#MainModal label[for='{fieldId}']").Locator("xpath=..");
        var control = field.Locator(".e-input-group.e-ddl");
        var selectedOption = control.Locator("select.e-ddl-hidden option:checked");

        await Expect(control).ToBeVisibleAsync();

        for (var attempt = 0; attempt < 3; attempt++)
        {
            if (await selectedOption.CountAsync() > 0 &&
                (await selectedOption.First.TextContentAsync())?.Trim() == optionText)
            {
                return;
            }

            await control.ClickAsync();

            var popup = Page.Locator(".e-popup.e-ddl.e-popup-open");
            await Expect(popup).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions
            {
                Timeout = 20000
            });

            await Page.WaitForTimeoutAsync(400);

            var option = popup.Locator(".e-list-item")
                .Filter(new LocatorFilterOptions { HasText = optionText })
                .First;

            await Expect(option).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions
            {
                Timeout = 20000
            });
            await option.ClickAsync();
            await Page.WaitForTimeoutAsync(200);
        }

        await Expect(selectedOption).ToHaveTextAsync(optionText, new LocatorAssertionsToHaveTextOptions
        {
            Timeout = 20000
        });
    }

    /// <summary>
    /// Clicks a toolbar button and waits for #MainModal to show. The list pages wire the modal up
    /// only after the lookup data and the grid finished loading, so an early click is swallowed.
    /// </summary>
    /// <param name="toolbarButtonId">Toolbar item id without the leading "#", for example "AddCustom".</param>
    protected async Task OpenModalAsync(string toolbarButtonId)
    {
        var modal = Page.Locator("#MainModal");
        var button = Page.Locator($"#{toolbarButtonId}");

        await Expect(button).ToBeEnabledAsync(new LocatorAssertionsToBeEnabledOptions
        {
            Timeout = 20000
        });

        for (var attempt = 0; attempt < 30 && !await modal.IsVisibleAsync(); attempt++)
        {
            await button.ClickAsync();
            await Page.WaitForTimeoutAsync(200);
        }

        await Expect(modal).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions
        {
            Timeout = 20000
        });
    }

    protected async Task ExpectNameRequiredMessageAsync()
    {
        await OpenModalAsync("AddCustom");
        await SubmitModalAsync();

        await Expect(Page.Locator("#MainModal label.text-danger").Filter(new LocatorFilterOptions
        {
            HasText = "Name is required."
        })).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions
        {
            Timeout = 20000
        });
        await Expect(Page.Locator("#MainModal")).ToBeVisibleAsync();

        await Page.Locator("#MainModal .modal-footer button[data-bs-dismiss=modal]").ClickAsync();
        await Expect(Page.Locator("#MainModal")).ToBeHiddenAsync();
    }

    protected async Task SelectRowAsync(string name)
    {
        var row = GridRow(name).First;
        await Expect(row).ToBeVisibleAsync();

        if (await row.GetAttributeAsync("aria-selected") != "true")
        {
            await row.GetByText(name, new LocatorGetByTextOptions { Exact = true }).ClickAsync();
        }

        await Expect(row).ToHaveAttributeAsync("aria-selected", "true", new LocatorAssertionsToHaveAttributeOptions
        {
            Timeout = 10000
        });
        await Expect(Page.Locator("#EditCustom")).ToBeEnabledAsync(new LocatorAssertionsToBeEnabledOptions
        {
            Timeout = 10000
        });
    }

    protected async Task CreateAsync(string name)
    {
        await OpenModalAsync("AddCustom");
        await FillFormAsync(name);
        await SubmitModalAsync();
        await ExpectSwalTitleAsync("Save Successful");
        await ExpectModalClosedAsync();
        await Expect(GridRow(name).First).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions
        {
            Timeout = 20000
        });
    }

    protected async Task UpdateAsync(string oldName, string newName)
    {
        await SelectRowAsync(oldName);
        await OpenModalAsync("EditCustom");
        await FillFormAsync(newName);
        await SubmitModalAsync();
        await ExpectSwalTitleAsync("Save Successful");
        await ExpectModalClosedAsync();
        await Expect(GridRow(newName).First).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions
        {
            Timeout = 20000
        });
        await Expect(GridRow(oldName)).ToHaveCountAsync(0);
    }

    protected async Task DeleteAsync(string name)
    {
        await SelectRowAsync(name);
        await OpenModalAsync("DeleteCustom");
        await SubmitModalAsync();
        await ExpectSwalTitleAsync("Delete Successful");
        await ExpectModalClosedAsync();
        await Expect(GridRow(name)).ToHaveCountAsync(0, new LocatorAssertionsToHaveCountOptions
        {
            Timeout = 20000
        });
    }
}
