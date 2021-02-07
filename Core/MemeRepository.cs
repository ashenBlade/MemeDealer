using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;

namespace Core
{
    public class MemeRepository
    {
        private List<Meme> Memes { get; set; }
        private Dictionary<string, List<Meme>> MemesWithTags { get; set; }

        private const string ImagesDirectoryName = "Images";
        private string ImagesDirectoryPath { get; }
        public MemeRepository()
        {
            using var db = new ApplicationContext();
            // Download memes from database
            Memes = db.Memes.ToList();

            // Get images folder full path
            ImagesDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), ImagesDirectoryName);

            // Update tags dictionary
            MemesWithTags = new Dictionary<string, List<Meme>>();
            foreach (Meme meme in Memes)
                UpdateDictionary(meme);
        }

        public void UpdateDictionary(Meme meme)
        {
            string[] tags = meme.Tags.Split();
            foreach (string tag in tags)
            {
                if (!(MemesWithTags.ContainsKey(tag)))
                    MemesWithTags.Add(tag, new List<Meme>());
                MemesWithTags[tag].Add(meme);
            }
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
            return MemesWithTags.Keys.ToList();
        }

        /// <summary>
        /// Adds a new meme to the database
        /// </summary>
        /// <param name="meme">New meme</param>
        public void Add(Meme meme)
        {
            CheckImagesDirectoryExists();
            MakeMemeBackup(meme);
            Memes.Add(meme);
            using var db = new ApplicationContext();
            db.Memes.Add(meme);
            UpdateDictionary(meme);
        }

        private void CheckImagesDirectoryExists()
        {
            if (!Directory.Exists(ImagesDirectoryName))
                Directory.CreateDirectory(ImagesDirectoryName);
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
            var result = new List<Meme>();
            if (requiredTags.Length == 0)
            {
                return Memes;
            }

            foreach (string tag in requiredTags)
            {
                if (MemesWithTags.ContainsKey(tag))
                {
                    foreach (Meme meme in MemesWithTags[tag])
                        if (!(result.Contains(meme)))
                            result.Add(meme);
                }
            }
            return result;
        }
    }
}