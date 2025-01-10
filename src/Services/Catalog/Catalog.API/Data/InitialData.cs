using Marten.Schema;

namespace Catalog.API.Data
{
    public class InitialData : IInitialData
    {
        public async Task Populate(IDocumentStore store, CancellationToken cancellation)
        {
            using var session = store.LightweightSession();

            if (await session.Query<Product>().AnyAsync(cancellation))
            {
                return;
            }

            session.Store<Product>(GetPreConfiguredProducts());
            await session.SaveChangesAsync(cancellation);
        }

        private IEnumerable<Product> GetPreConfiguredProducts()
        {
            return
            [
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Wireless Mouse",
                    Description = "A sleek and modern wireless mouse with ergonomic design.",
                    Category = ["Electronics", "Accessories"],
                    ImageFile = "wireless_mouse.jpg",
                    Price = 25.99m
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Gaming Keyboard",
                    Description = "Mechanical keyboard with RGB backlighting for gaming enthusiasts.",
                    Category = ["Electronics", "Gaming"],
                    ImageFile = "gaming_keyboard.jpg",
                    Price = 49.99m
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Noise-Cancelling Headphones",
                    Description = "Over-ear headphones with active noise cancellation and superior sound quality.",
                    Category = ["Electronics", "Audio"],
                    ImageFile = "headphones.jpg",
                    Price = 199.99m
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Smartphone Holder",
                    Description = "Adjustable smartphone holder for cars, compatible with all phone models.",
                    Category = ["Accessories", "Automotive"],
                    ImageFile = "smartphone_holder.jpg",
                    Price = 15.49m
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Portable Charger",
                    Description = "10,000mAh power bank with fast-charging capabilities and dual USB ports.",
                    Category = ["Electronics", "Accessories"],
                    ImageFile = "portable_charger.jpg",
                    Price = 29.99m
                }
            ];
        }
    }
}
