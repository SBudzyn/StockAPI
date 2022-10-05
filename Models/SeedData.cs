using StockAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace StockAPI.Models
{
    public class SeedData
    {
        public static async Task InitializeTexts(IServiceProvider provider)
        {

            using (var context = new StockContext(provider.GetRequiredService<DbContextOptions<StockContext>>()))
            {
                if (context == null || context.Texts == null)
                {
                    return;
                }

                if (context.Texts.Any())
                {
                    return;
                }
                Author tom = new Author
                {
                    FirstName = "Tom",
                    LastName = "Nabe",
                    DateOfBirth = new DateTime(1980, 11, 11),
                    DateOfRegistration = new DateTime(2022, 9, 21),
                    NickName = "Tommmmm"
                };
                Author bob = new Author
                {
                    FirstName = "Bob",
                    LastName = "Goa",
                    DateOfBirth = new DateTime(1985, 5, 30),
                    DateOfRegistration = new DateTime(2022, 9, 16),
                    NickName = "Bobino"
                };
                Author sam = new Author
                {
                    FirstName = "Sam",
                    LastName = "Horse",
                    DateOfBirth = new DateTime(1984, 6, 12),
                    DateOfRegistration = new DateTime(2022, 8, 24),
                    NickName = "Sad sam"
                };
                context.AddRange(tom, sam, bob);
                context.Texts.AddRange(new Text 
                {
                    Author = tom,
                    Cost = 100m,
                    Name = "Walking in the forest",
                    DateOfCreation = new DateTime(2021, 12, 8),
                    NumberOfSales = 1243,
                    TextInfo = "Lorem ipsum dolore covaro. Lorem ipsum daviaro."
                },
                new Text
                {
                    Author = bob,
                    Cost = 87m,
                    Name = "Existance of the human",
                    DateOfCreation = new DateTime(2008, 1, 27),
                    NumberOfSales = 67943,
                    TextInfo = "Polore viaro. Lorem ipsum dolore covaro. Lorem ipsum daviaro."
                },
                new Text
                {
                    Author = sam,
                    Cost = 67m,
                    Name = "Adulhood",
                    DateOfCreation = new DateTime(2012, 3, 13),
                    NumberOfSales = 6943,
                    TextInfo = "Romano de voro. Contaci firoro. Lorem ipsum dolore covaro. Lorem ipsum daviaro."
                },
                new Text
                {
                    Author = tom,
                    Cost = 76m,
                    Name = "My path to fame",
                    DateOfCreation = new DateTime(2014, 6, 17),
                    NumberOfSales = 12943,
                    TextInfo = "Calamaro de nunciato. Contaci firoro. Lorem ipsum dolore covaro. Lorem ipsum daviaro."
                });
                await context.SaveChangesAsync();


            }
        }
        public static async Task InitializePhotos(IServiceProvider provider)
        {
            using (var context = new StockContext(provider.GetRequiredService<DbContextOptions<StockContext>>()))
            {
                if (context == null)
                {
                    return;
                }
                if (context.Photos.Any())
                {
                    return;
                }
                await context.Photos.AddRangeAsync(new Photo 
                {
                    Author = context.Authors.FirstOrDefault(a => a.Id == 2)!,
                    Name = "Rainy city",
                    DateOfCreation= new DateTime(2009, 12, 12),
                    Cost = 12m,
                    Link = "Rain.jpg",
                    NumberOfSales = 23423,
                    OriginalSize = 0
                },
                new Photo
                {
                    Author = context.Authors.FirstOrDefault(a => a.Id == 3)!,
                    Name = "My dream",
                    DateOfCreation = new DateTime(2012, 10, 1),
                    Cost = 22m,
                    Link = "Dream.jpg",
                    NumberOfSales = 54433,
                    OriginalSize = 0
                }
                );
                await context.SaveChangesAsync();

            }
        }
    }
}
