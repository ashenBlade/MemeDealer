using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Core
{
    public class MemeRepository
    {
        private List<Meme> Memes { get; set; }
        public MemeRepository()
        {
            using var db = new ApplicationContext();
            Memes = db.Memes.ToList();
        }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void Remove(Meme meme)
        {
            throw new NotImplementedException();
        }

        public List<Meme> GetAllMemes()
        {
            throw new NotImplementedException();
        }

        public List<string> GetAllTags()
        {
            throw new NotImplementedException();
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

            Memes.Add(newMeme);
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
