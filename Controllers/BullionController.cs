using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace test_api.Controllers;


public class NewBullion
{
    public string Name { get; set; } = null!;
    public decimal Weight { get; set; }
}

public class BullionId
{
    public long Id { get; set; }
}
[ApiController]
[Route("[controller]")]
public class BullionController : ControllerBase
{
    private readonly ILogger<BullionController> _logger;
    private readonly BullionContext _context;

    public BullionController(ILogger<BullionController> logger, BullionContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet("FreeBullions")]
    public IEnumerable<Bullion> GetFree()
    {
        var lstExcluded = _context.Baskets.Include(x => x.Bullions).AsNoTracking().First().Bullions.Select(x => x.Id).ToArray();
        return _context.Bullions.AsNoTracking().Where(x => !lstExcluded.Contains(x.Id));
    }
    [HttpGet("BullionsInBasket")]
    public IEnumerable<Bullion> GetBullionsInBasket()
    {
        return _context.Baskets.Include(x => x.Bullions).AsNoTracking().First().Bullions;
    }


    [HttpPost("AddNewBullion")]
    public IActionResult AddNewBullion([FromBody] NewBullion newBullion)
    {
        if (newBullion == null)
        {
            return BadRequest("Не передан объект с новой слитком");
        }
        if (newBullion.Weight <= 0)
        {
            return BadRequest("Bec слитка должен быть положительным");
        }
        if (_context.Bullions.FirstOrDefault(x => x.Name == newBullion.Name) != null)
        {
            return Conflict("Слиток с таким именем есть");
        }
        var entity = new Bullion
        {
            Name = newBullion.Name,
            Weight = newBullion.Weight
        };
        _context.Add(entity);
        _context.SaveChanges();
        return Ok(entity);
    }

    [HttpPost("AddNewBullionToBasket")]
    public IActionResult AddNewToBasket([FromBody] NewBullion newBullion)
    {
        if (newBullion == null)
        {
            return BadRequest("Не передан объект с новой слитком");
        }
        if (newBullion.Weight <= 0)
        {
            return BadRequest("Bec слитка должен быть положительным");
        }
        if (_context.Bullions.FirstOrDefault(x => x.Name == newBullion.Name) != null)
        {
            return Conflict("Слиток с таким именем есть");
        }
        var entity = new Bullion
        {
            Name = newBullion.Name,
            Weight = newBullion.Weight
        };
        var basket = _context.Baskets.Include(x => x.Bullions).First();
        basket.Bullions.Add(entity);
        _context.SaveChanges();
        return Ok(entity);
    }
    [HttpPost("AddBullionToBasket")]
    public IActionResult AddBullionToBasket([FromBody] BullionId bullionId)
    {
        var entity = _context.Bullions.FirstOrDefault(x => x.Id == bullionId.Id);
        if (entity == null)
        {
            return NotFound("Слиток не найден");
        }

        var basket = _context.Baskets.Include(x => x.Bullions).First();
        basket.Bullions.Add(entity);
        _context.SaveChanges();
        return Ok(entity);
    }

    // [HttpGet(Name = "GetWeatherForecast")]
    // public IEnumerable<Bullion> Get()
    // {
    //     return Enumerable.Range(1, 5).Select(index => new Bullion
    //     {
    //         Date = DateTime.Now.AddDays(index),
    //         TemperatureC = Random.Shared.Next(-20, 55),
    //         Summary = Summaries[Random.Shared.Next(Summaries.Length)]
    //     })
    //     .ToArray();
    // }
}
