using System;

namespace Core
{
    class Example
    {
        static void Main()
        {
            // Примеры работы:
            MemeRepository dbt = new MemeRepository();

            dbt.Add(new Meme
            {
                Name = "my cat",
                Description = "beautiful cat",
                Tags = "animals cats",
                PathToFile = "images/mycat.png"
            });
            dbt.Add(new Meme
            {
                Name = "my dog",
                Description = "big black dog",
                Tags = "animals dogs",
                PathToFile = "images/mydog.png"
            });
            dbt.Add(new Meme
            {
                Name = "the biggest cat",
                Tags = "animals cats biggest",
                PathToFile = "images/biggestCat.png"
            });

            foreach (Meme img in dbt.FindByTags("")) // my cat, my dog, the biggest cat
            {
                Console.WriteLine($"{img.Id}. {img.Name}");
            }

            foreach (Meme img in dbt.FindByTags("cats")) // my cat, the biggest cat
            {
                Console.WriteLine($"{img.Id}. {img.Name}");
            }

            foreach (Meme img in dbt.FindByTags("dogs")) // my dog
            {
                Console.WriteLine($"{img.Id}. {img.Name}");
            }

            foreach (Meme img in dbt.FindByTags("animals")) // my cat, my dog, the biggest cat
            {
                Console.WriteLine($"{img.Id}. {img.Name}");
            }
        }
    }
}
