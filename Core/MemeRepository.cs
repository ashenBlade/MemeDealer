﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Core
{
    public class MemeRepository
    {
        private List<Meme> Memes { get; set; }
        private Dictionary<string, List<Meme>> MemesWithTags{ get; set; }

        private const string ImagesDirectoryName = "Images";
        private string ImagesDirectoryPath { get; }
        public MemeRepository()
        {
            using var db = new ApplicationContext();
            Memes = db.Memes.ToList();
            ImagesDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), ImagesDirectoryName);
            foreach (Meme meme in Memes)
            {
                UpdateDictionary(meme);
            }
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
            MakeMemeBackup(meme);
            Memes.Add(meme);
            using var db = new ApplicationContext();
            db.Memes.Add(meme);
            UpdateDictionary(meme);
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
        public List<List<Meme>> FindByTags(string tagsString)
        {
            string[] requiredTags = tagsString.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var result = new List<List<Meme>>();
            if (requiredTags.Length == 0)
            {
                result.Add(Memes);
                return result;
            }
            foreach (Meme meme in Memes)
            {
                string[] tags = meme.Tags.Split();

                foreach (string tag in tags)
                {
                    if (MemesWithTags.ContainsKey(tag))
                        result.Add(MemesWithTags[tag]);
                }
            }
            return result;
        }
    }
}
