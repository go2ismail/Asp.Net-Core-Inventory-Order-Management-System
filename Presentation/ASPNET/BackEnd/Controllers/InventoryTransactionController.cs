using Application.Features.InventoryTransactionManager.Commands;
using Application.Features.InventoryTransactionManager.Queries;
using ASPNET.BackEnd.Common.Base;
using ASPNET.BackEnd.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET.BackEnd.Controllers;

[Route("api/[controller]")]
public class InventoryTransactionController : BaseApiController
{
    public InventoryTransactionController(ISender sender) : base(sender)
    {
    }



    [Authorize]
    [HttpGet("GetInventoryTransactionList")]
    public async Task<ActionResult<ApiSuccessResult<GetInventoryTransactionListResult>>> GetInventoryTransactionListAsync(
        CancellationToken cancellationToken
        )
    {
        var request = new GetInventoryTransactionListRequest { };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetInventoryTransactionListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetInventoryTransactionListAsync)}",
            Content = response
        });
    }


    [Authorize]
    [HttpGet("GetInventoryStockList")]
    public async Task<ActionResult<ApiSuccessResult<GetInventoryStockListResult>>> GetInventoryStockListAsync(
        CancellationToken cancellationToken
        )
    {
        var request = new GetInventoryStockListRequest { };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GetInventoryStockListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GetInventoryStockListAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("DeliveryOrderCreateInvenTrans")]
    public async Task<ActionResult<ApiSuccessResult<DeliveryOrderCreateInvenTransResult>>> DeliveryOrderCreateInvenTransAsync(DeliveryOrderCreateInvenTransRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<DeliveryOrderCreateInvenTransResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(DeliveryOrderCreateInvenTransAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("DeliveryOrderUpdateInvenTrans")]
    public async Task<ActionResult<ApiSuccessResult<DeliveryOrderUpdateInvenTransResult>>> DeliveryOrderUpdateInvenTransAsync(DeliveryOrderUpdateInvenTransRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<DeliveryOrderUpdateInvenTransResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(DeliveryOrderUpdateInvenTransAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("DeliveryOrderDeleteInvenTrans")]
    public async Task<ActionResult<ApiSuccessResult<DeliveryOrderDeleteInvenTransResult>>> DeliveryOrderDeleteInvenTransAsync(DeliveryOrderDeleteInvenTransRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<DeliveryOrderDeleteInvenTransResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(DeliveryOrderDeleteInvenTransAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpGet("DeliveryOrderGetInvenTransList")]
    public async Task<ActionResult<ApiSuccessResult<DeliveryOrderGetInvenTransListResult>>> DeliveryOrderGetInvenTransListAsync(
        CancellationToken cancellationToken,
        [FromQuery] string moduleId
        )
    {
        var request = new DeliveryOrderGetInvenTransListRequest { ModuleId = moduleId };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<DeliveryOrderGetInvenTransListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(DeliveryOrderGetInvenTransListAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("SalesReturnCreateInvenTrans")]
    public async Task<ActionResult<ApiSuccessResult<SalesReturnCreateInvenTransResult>>> SalesReturnCreateInvenTransAsync(SalesReturnCreateInvenTransRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<SalesReturnCreateInvenTransResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(SalesReturnCreateInvenTransAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("SalesReturnUpdateInvenTrans")]
    public async Task<ActionResult<ApiSuccessResult<SalesReturnUpdateInvenTransResult>>> SalesReturnUpdateInvenTransAsync(SalesReturnUpdateInvenTransRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<SalesReturnUpdateInvenTransResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(SalesReturnUpdateInvenTransAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("SalesReturnDeleteInvenTrans")]
    public async Task<ActionResult<ApiSuccessResult<SalesReturnDeleteInvenTransResult>>> SalesReturnDeleteInvenTransAsync(SalesReturnDeleteInvenTransRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<SalesReturnDeleteInvenTransResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(SalesReturnDeleteInvenTransAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpGet("SalesReturnGetInvenTransList")]
    public async Task<ActionResult<ApiSuccessResult<SalesReturnGetInvenTransListResult>>> SalesReturnGetInvenTransListAsync(
        CancellationToken cancellationToken,
        [FromQuery] string moduleId
        )
    {
        var request = new SalesReturnGetInvenTransListRequest { ModuleId = moduleId };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<SalesReturnGetInvenTransListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(SalesReturnGetInvenTransListAsync)}",
            Content = response
        });
    }


    [Authorize]
    [HttpPost("GoodsReceiveCreateInvenTrans")]
    public async Task<ActionResult<ApiSuccessResult<GoodsReceiveCreateInvenTransResult>>> GoodsReceiveCreateInvenTransAsync(GoodsReceiveCreateInvenTransRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GoodsReceiveCreateInvenTransResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GoodsReceiveCreateInvenTransAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("GoodsReceiveUpdateInvenTrans")]
    public async Task<ActionResult<ApiSuccessResult<GoodsReceiveUpdateInvenTransResult>>> GoodsReceiveUpdateInvenTransAsync(GoodsReceiveUpdateInvenTransRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GoodsReceiveUpdateInvenTransResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GoodsReceiveUpdateInvenTransAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("GoodsReceiveDeleteInvenTrans")]
    public async Task<ActionResult<ApiSuccessResult<GoodsReceiveDeleteInvenTransResult>>> GoodsReceiveDeleteInvenTransAsync(GoodsReceiveDeleteInvenTransRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GoodsReceiveDeleteInvenTransResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GoodsReceiveDeleteInvenTransAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpGet("GoodsReceiveGetInvenTransList")]
    public async Task<ActionResult<ApiSuccessResult<GoodsReceiveGetInvenTransListResult>>> GoodsReceiveGetInvenTransListAsync(
        CancellationToken cancellationToken,
        [FromQuery] string moduleId
        )
    {
        var request = new GoodsReceiveGetInvenTransListRequest { ModuleId = moduleId };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<GoodsReceiveGetInvenTransListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(GoodsReceiveGetInvenTransListAsync)}",
            Content = response
        });
    }


    [Authorize]
    [HttpPost("PurchaseReturnCreateInvenTrans")]
    public async Task<ActionResult<ApiSuccessResult<PurchaseReturnCreateInvenTransResult>>> PurchaseReturnCreateInvenTransAsync(PurchaseReturnCreateInvenTransRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<PurchaseReturnCreateInvenTransResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(PurchaseReturnCreateInvenTransAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("PurchaseReturnUpdateInvenTrans")]
    public async Task<ActionResult<ApiSuccessResult<PurchaseReturnUpdateInvenTransResult>>> PurchaseReturnUpdateInvenTransAsync(PurchaseReturnUpdateInvenTransRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<PurchaseReturnUpdateInvenTransResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(PurchaseReturnUpdateInvenTransAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("PurchaseReturnDeleteInvenTrans")]
    public async Task<ActionResult<ApiSuccessResult<PurchaseReturnDeleteInvenTransResult>>> PurchaseReturnDeleteInvenTransAsync(PurchaseReturnDeleteInvenTransRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<PurchaseReturnDeleteInvenTransResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(PurchaseReturnDeleteInvenTransAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpGet("PurchaseReturnGetInvenTransList")]
    public async Task<ActionResult<ApiSuccessResult<PurchaseReturnGetInvenTransListResult>>> PurchaseReturnGetInvenTransListAsync(
        CancellationToken cancellationToken,
        [FromQuery] string moduleId
        )
    {
        var request = new PurchaseReturnGetInvenTransListRequest { ModuleId = moduleId };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<PurchaseReturnGetInvenTransListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(PurchaseReturnGetInvenTransListAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("TransferOutCreateInvenTrans")]
    public async Task<ActionResult<ApiSuccessResult<TransferOutCreateInvenTransResult>>> TransferOutCreateInvenTransAsync(TransferOutCreateInvenTransRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<TransferOutCreateInvenTransResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(TransferOutCreateInvenTransAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("TransferOutUpdateInvenTrans")]
    public async Task<ActionResult<ApiSuccessResult<TransferOutUpdateInvenTransResult>>> TransferOutUpdateInvenTransAsync(TransferOutUpdateInvenTransRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<TransferOutUpdateInvenTransResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(TransferOutUpdateInvenTransAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("TransferOutDeleteInvenTrans")]
    public async Task<ActionResult<ApiSuccessResult<TransferOutDeleteInvenTransResult>>> TransferOutDeleteInvenTransAsync(TransferOutDeleteInvenTransRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<TransferOutDeleteInvenTransResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(TransferOutDeleteInvenTransAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpGet("TransferOutGetInvenTransList")]
    public async Task<ActionResult<ApiSuccessResult<TransferOutGetInvenTransListResult>>> TransferOutGetInvenTransListAsync(
        CancellationToken cancellationToken,
        [FromQuery] string moduleId
        )
    {
        var request = new TransferOutGetInvenTransListRequest { ModuleId = moduleId };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<TransferOutGetInvenTransListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(TransferOutGetInvenTransListAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("TransferInCreateInvenTrans")]
    public async Task<ActionResult<ApiSuccessResult<TransferInCreateInvenTransResult>>> TransferInCreateInvenTransAsync(TransferInCreateInvenTransRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<TransferInCreateInvenTransResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(TransferInCreateInvenTransAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("TransferInUpdateInvenTrans")]
    public async Task<ActionResult<ApiSuccessResult<TransferInUpdateInvenTransResult>>> TransferInUpdateInvenTransAsync(TransferInUpdateInvenTransRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<TransferInUpdateInvenTransResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(TransferInUpdateInvenTransAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("TransferInDeleteInvenTrans")]
    public async Task<ActionResult<ApiSuccessResult<TransferInDeleteInvenTransResult>>> TransferInDeleteInvenTransAsync(TransferInDeleteInvenTransRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<TransferInDeleteInvenTransResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(TransferInDeleteInvenTransAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpGet("TransferInGetInvenTransList")]
    public async Task<ActionResult<ApiSuccessResult<TransferInGetInvenTransListResult>>> TransferInGetInvenTransListAsync(
        CancellationToken cancellationToken,
        [FromQuery] string moduleId
        )
    {
        var request = new TransferInGetInvenTransListRequest { ModuleId = moduleId };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<TransferInGetInvenTransListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(TransferInGetInvenTransListAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("PositiveAdjustmentCreateInvenTrans")]
    public async Task<ActionResult<ApiSuccessResult<PositiveAdjustmentCreateInvenTransResult>>> PositiveAdjustmentCreateInvenTransAsync(PositiveAdjustmentCreateInvenTransRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<PositiveAdjustmentCreateInvenTransResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(PositiveAdjustmentCreateInvenTransAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("PositiveAdjustmentUpdateInvenTrans")]
    public async Task<ActionResult<ApiSuccessResult<PositiveAdjustmentUpdateInvenTransResult>>> PositiveAdjustmentUpdateInvenTransAsync(PositiveAdjustmentUpdateInvenTransRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<PositiveAdjustmentUpdateInvenTransResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(PositiveAdjustmentUpdateInvenTransAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("PositiveAdjustmentDeleteInvenTrans")]
    public async Task<ActionResult<ApiSuccessResult<PositiveAdjustmentDeleteInvenTransResult>>> PositiveAdjustmentDeleteInvenTransAsync(PositiveAdjustmentDeleteInvenTransRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<PositiveAdjustmentDeleteInvenTransResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(PositiveAdjustmentDeleteInvenTransAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpGet("PositiveAdjustmentGetInvenTransList")]
    public async Task<ActionResult<ApiSuccessResult<PositiveAdjustmentGetInvenTransListResult>>> PositiveAdjustmentGetInvenTransListAsync(
        CancellationToken cancellationToken,
        [FromQuery] string moduleId
        )
    {
        var request = new PositiveAdjustmentGetInvenTransListRequest { ModuleId = moduleId };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<PositiveAdjustmentGetInvenTransListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(PositiveAdjustmentGetInvenTransListAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("NegativeAdjustmentCreateInvenTrans")]
    public async Task<ActionResult<ApiSuccessResult<NegativeAdjustmentCreateInvenTransResult>>> NegativeAdjustmentCreateInvenTransAsync(NegativeAdjustmentCreateInvenTransRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<NegativeAdjustmentCreateInvenTransResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(NegativeAdjustmentCreateInvenTransAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("NegativeAdjustmentUpdateInvenTrans")]
    public async Task<ActionResult<ApiSuccessResult<NegativeAdjustmentUpdateInvenTransResult>>> NegativeAdjustmentUpdateInvenTransAsync(NegativeAdjustmentUpdateInvenTransRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<NegativeAdjustmentUpdateInvenTransResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(NegativeAdjustmentUpdateInvenTransAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("NegativeAdjustmentDeleteInvenTrans")]
    public async Task<ActionResult<ApiSuccessResult<NegativeAdjustmentDeleteInvenTransResult>>> NegativeAdjustmentDeleteInvenTransAsync(NegativeAdjustmentDeleteInvenTransRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<NegativeAdjustmentDeleteInvenTransResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(NegativeAdjustmentDeleteInvenTransAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpGet("NegativeAdjustmentGetInvenTransList")]
    public async Task<ActionResult<ApiSuccessResult<NegativeAdjustmentGetInvenTransListResult>>> NegativeAdjustmentGetInvenTransListAsync(
        CancellationToken cancellationToken,
        [FromQuery] string moduleId
        )
    {
        var request = new NegativeAdjustmentGetInvenTransListRequest { ModuleId = moduleId };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<NegativeAdjustmentGetInvenTransListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(NegativeAdjustmentGetInvenTransListAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("ScrappingCreateInvenTrans")]
    public async Task<ActionResult<ApiSuccessResult<ScrappingCreateInvenTransResult>>> ScrappingCreateInvenTransAsync(ScrappingCreateInvenTransRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<ScrappingCreateInvenTransResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(ScrappingCreateInvenTransAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("ScrappingUpdateInvenTrans")]
    public async Task<ActionResult<ApiSuccessResult<ScrappingUpdateInvenTransResult>>> ScrappingUpdateInvenTransAsync(ScrappingUpdateInvenTransRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<ScrappingUpdateInvenTransResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(ScrappingUpdateInvenTransAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("ScrappingDeleteInvenTrans")]
    public async Task<ActionResult<ApiSuccessResult<ScrappingDeleteInvenTransResult>>> ScrappingDeleteInvenTransAsync(ScrappingDeleteInvenTransRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<ScrappingDeleteInvenTransResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(ScrappingDeleteInvenTransAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpGet("ScrappingGetInvenTransList")]
    public async Task<ActionResult<ApiSuccessResult<ScrappingGetInvenTransListResult>>> ScrappingGetInvenTransListAsync(
        CancellationToken cancellationToken,
        [FromQuery] string moduleId
        )
    {
        var request = new ScrappingGetInvenTransListRequest { ModuleId = moduleId };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<ScrappingGetInvenTransListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(ScrappingGetInvenTransListAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("StockCountCreateInvenTrans")]
    public async Task<ActionResult<ApiSuccessResult<StockCountCreateInvenTransResult>>> StockCountCreateInvenTransAsync(StockCountCreateInvenTransRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<StockCountCreateInvenTransResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(StockCountCreateInvenTransAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("StockCountUpdateInvenTrans")]
    public async Task<ActionResult<ApiSuccessResult<StockCountUpdateInvenTransResult>>> StockCountUpdateInvenTransAsync(StockCountUpdateInvenTransRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<StockCountUpdateInvenTransResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(StockCountUpdateInvenTransAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpPost("StockCountDeleteInvenTrans")]
    public async Task<ActionResult<ApiSuccessResult<StockCountDeleteInvenTransResult>>> StockCountDeleteInvenTransAsync(StockCountDeleteInvenTransRequest request, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<StockCountDeleteInvenTransResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(StockCountDeleteInvenTransAsync)}",
            Content = response
        });
    }

    [Authorize]
    [HttpGet("StockCountGetInvenTransList")]
    public async Task<ActionResult<ApiSuccessResult<StockCountGetInvenTransListResult>>> StockCountGetInvenTransListAsync(
        CancellationToken cancellationToken,
        [FromQuery] string moduleId
        )
    {
        var request = new StockCountGetInvenTransListRequest { ModuleId = moduleId };
        var response = await _sender.Send(request, cancellationToken);

        return Ok(new ApiSuccessResult<StockCountGetInvenTransListResult>
        {
            Code = StatusCodes.Status200OK,
            Message = $"Success executing {nameof(StockCountGetInvenTransListAsync)}",
            Content = response
        });
    }

}


