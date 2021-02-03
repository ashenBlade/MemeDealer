using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Core
{
    public class MemeRepository
    {
        readonly ApplicationContext db;

        public MemeRepository()
        {
            db = new ApplicationContext();
        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }

        public void Clear()
        {
            db.Memes.RemoveRange(db.Memes);
        }

        public void Remove(Meme meme)
        {
            db.Memes.Remove(meme);
        }

        public List<Meme> GetAll()
        {
            return db.Memes.ToList();
        }

        /// <summary>
        /// Adds a new meme to the database
        /// </summary>
        /// <param name="newMeme">new meme</param>
        public void Add(Meme newMeme)
        {
            FileInfo oldFile = new FileInfo(newMeme.PathToFile);
            string destDir = "Images";

            if (!Directory.Exists(destDir))
                Directory.CreateDirectory(destDir);

            string newFileName = newMeme.Name + oldFile.Extension;
            string newPathToFile = Path.Combine(destDir, newFileName);

            oldFile.CopyTo(newPathToFile, true);

            newMeme.PathToFile = newPathToFile;

            db.Memes.Add(newMeme);
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
                return db.Memes.ToList();
            }

            List<Meme> result = new List<Meme>();

            foreach (Meme meme in db.Memes.ToList())
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
