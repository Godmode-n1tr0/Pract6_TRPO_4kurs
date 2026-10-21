using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Pract6.Models;

namespace Pract6.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class MedicinesController : ControllerBase
    {
        private readonly WebApiDatabaseContext db;

        public MedicinesController(WebApiDatabaseContext context)
        {
            db = context;
        }

        [HttpGet(Name = "Medicines")]
        public IActionResult GetAll(string? manufacture = null, bool? available = null)
        {
            var medicines = db.Medicines.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(manufacture))
            {
                medicines = medicines.Where(m => m.Manufacturer == manufacture);
            }
            if (available.HasValue)
            {
                medicines = medicines.Where(m => m.IsAvailable == available.Value);
            }
            return Ok(medicines.OrderBy(m => m.Id).ToList());
        }

        [HttpGet("{id:int}", Name = "MedicinesWithId")]
        public IActionResult Get(int id)
        {
            var medicine = db.Medicines.Find(id);
            if (medicine == null)
            {
                return NotFound("Лекарство не нашлось.");
            }
            return Ok(medicine);
        }

        [HttpPost(Name = "AddMedicines")]
        public IActionResult Create(Medicine medicine)
        {
            if (medicine.Id != 0)
            {
                return BadRequest("При добавление нужно указывать айди продукта 0!");
            }

            db.Medicines.Add(medicine);
            db.SaveChanges();

            return CreatedAtRoute("MedicinesWithId", new
            {
                id = medicine.Id,
            }, medicine);
        }

        [HttpPut("{id:int}", Name = "PutMedicines")]
        public IActionResult Put(int id, Medicine medicine)
        {
            if (id != medicine.Id)
            {
                return BadRequest("Id в адресе и в json должны быть одинаковыми");
            }
            var current = db.Medicines.Find(id);
            
            if (current == null)
            {
                return NotFound("Лекарства не найдено.");
            }

            db.Entry(current).CurrentValues.SetValues(medicine);
            db.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id:int}", Name = "PatchMedicines")]
        public IActionResult Patch(int id, MedicineUpdate changes)
        {
            var medicine = db.Medicines.Find(id);

            if (medicine == null)
            {
                return NotFound("Лекарство не найдено.");
            }

            if (changes.Name == null &&
                changes.Manufacturer == null &&
                changes.Price == null &&
                changes.ExpirationDate == null &&
                changes.IsAvailable == null)
            {
                return BadRequest("Укажи хотя бы одно поле для изменения.");
            }

            if (changes.Name != null)
            {
                if(changes.Name != "string")
                {
                    medicine.Name = changes.Name;
                }
                else
                {
                    changes.Name = medicine.Name;
                }
            }

            if (changes.Manufacturer != null)
            {
                if (changes.Manufacturer != "string")
                {
                    medicine.Manufacturer = changes.Manufacturer;
                }
                else
                {
                    changes.Manufacturer = medicine.Manufacturer;
                }
            }

            if (changes.Price.HasValue)
            {
                if (changes.Price >= 0)
                {
                    medicine.Price = changes.Price.Value;
                }
                else
                {
                    changes.Price = medicine.Price;
                    return BadRequest("Вы не можете указать отрицательную цену!");
                }
            }

            if (changes.ExpirationDate.HasValue)
            {
                medicine.ExpirationDate = changes.ExpirationDate.Value;
            }

            if (changes.IsAvailable.HasValue)
            {
                medicine.IsAvailable = changes.IsAvailable.Value;
            }

            db.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id:int}", Name = "DeleteMedicines")]
        public IActionResult Delete(int id)
        {
            var medicine = db.Medicines.Find(id);
            if (medicine == null)
            {
                return NotFound("Лекарства нету.");
            }

            db.Medicines.Remove(medicine);
            db.SaveChanges();

            return NoContent();
        }
    }
}
