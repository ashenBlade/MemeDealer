using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Core
{
    public class MemeRepository
    {
        private List<Meme> Memes { get; set; }

        private const string ImagesDirectoryName = "Images";
        private string ImagesDirectoryPath { get; }
        public MemeRepository()
        {
            using var db = new ApplicationContext();
            Memes = db.Memes.ToList();
            ImagesDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), ImagesDirectoryName);
        }

        public void SaveChanges()
        {
            using var db = new ApplicationContext();
            db.Memes.UpdateRange(Memes);
            db.SaveChanges();
        }

        // Why?
        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void Remove(Meme meme)
        {
            Memes.Remove(meme);
        }

        public List<Meme> GetAllMemes()
        {
            return Memes;
        }

        public List<string> GetAllTags()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a new meme to the database
        /// </summary>
        /// <param name="meme">New meme</param>
        public void Add(Meme meme)
        {
            MakeMemeBackup(meme);
            Memes.Add(meme);
            using var db = new ApplicationContext();
            db.Memes.Add(meme);
        }

        private void MakeMemeBackup(Meme meme)
        {
            var originalFile = new FileInfo(meme.PathToFile);
            var newFilename = string.Concat(meme.Name, originalFile.Extension);
            var newFilepath = Path.Combine(ImagesDirectoryPath, newFilename);
            originalFile.CopyTo(newFilepath, true);
            meme.PathToFile = newFilepath;
        }

        /// <summary>
        /// Finds memes by tags
        /// </summary>
        /// <param name="tagsString">Tags separated by a space</param>
        /// <returns>If an empty string is passed, it will return all memes.
        ///          Otherwise, it will return memes that have at least one required tag</returns>
        public List<Meme> FindByTags(string tagsString)
        {
            string[] requiredTags = tagsString.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (requiredTags.Length == 0)
            {
                return Memes;
            }

            List<Meme> result = new List<Meme>();

            foreach (Meme meme in Memes)
            {
                string[] tags = meme.Tags.Split();

                foreach (string tag in tags)
                {
                    bool found = false;
                    foreach (string requiredTag in requiredTags)
                    {
                        if (tag == requiredTag)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (found)
                    {
                        result.Add(meme);
                        break;
                    }
                }
            }

            return result;
        }
    }
}
