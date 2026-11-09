namespace Application.Common.Services.SecurityManager;

public class MenuNavigationTreeNodeDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string? Pid { get; set; }
    public string? NavURL { get; set; }
    public bool HasChild { get; set; }
    public bool Expanded { get; set; }
    public bool IsSelected { get; set; }

    public MenuNavigationTreeNodeDto(string param_id, string param_name, string? param_pid = null, string? param_navURL = null, bool param_hasChild = false, bool param_expanded = false, bool param_selected = false)
    {
        Id = param_id;
        Name = param_name;
        Pid = param_pid;
        NavURL = param_navURL;
        HasChild = param_hasChild;
        Expanded = param_expanded;
        IsSelected = param_selected;
    }
}
