using NUnit.Framework;
using Core;

namespace Tests
{
    public class Core_Tests
    {
        DatabaseTools dbt;

        [SetUp]
        public void Setup()
        {
            dbt = new DatabaseTools();
        }

        [Test]
        public void Add_Tests()
        {
            dbt.Clear();

            dbt.Add(new Image
            {
                Name = "my cat",
                Tags = "animals cats",
            });
            dbt.Add(new Image
            {
                Name = "my dog",
                Tags = "animals dogs",
            });
            dbt.Add(new Image
            {
                Name = "the biggest cat",
                Tags = "animals cats biggest",
            });

            Assert.AreEqual(new[] { "my cat", "the biggest cat" }, dbt.FindByTags("cats").ConvertAll(x => x.Name).ToArray());

            Assert.AreEqual(new[] { "my cat", "my dog", "the biggest cat" }, dbt.FindByTags("").ConvertAll(x => x.Name).ToArray());

            Assert.AreEqual(new[] { "the biggest cat" }, dbt.FindByTags("biggest").ConvertAll(x => x.Name).ToArray());
        }
    }
}