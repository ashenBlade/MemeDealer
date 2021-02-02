using NUnit.Framework;
using Core;
using System.IO;

namespace Tests
{
    public class Core_Tests
    {
        MemeRepository db;

        [SetUp]
        public void Setup()
        {
            db = new MemeRepository();
        }

        [Test]
        public void Add_Copy_Tests()
        {
            db.Clear();

            if (Directory.Exists("Images"))
                Directory.Delete("Images", true);

            db.Add(new Meme
            {
                Name = "Бургеры",
                Description = "Технари готовят бургеры",
                Tags = "студенты бедность макдональдс работа",
                PathToFile = "memes/first.jpg"
            });

            db.SaveChanges();

            Assert.IsTrue(File.Exists("Images/Бургеры.jpg"));
        }
    }
}