using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class DatabaseTools
    {
        public void Clear()
        {
            ApplicationContext db = new ApplicationContext();
            db.Images.RemoveRange(db.Images);
            db.SaveChanges();
        }

        /// <summary>
        /// Adds a new image to the database
        /// </summary>
        /// <param name="newImage">new image</param>
        public void Add(Image newImage)
        {
            ApplicationContext db = new ApplicationContext();
            db.Images.Add(newImage);

            db.SaveChanges();
        }

        /// <summary>
        /// Finds images by tags
        /// </summary>
        /// <param name="tagsString">Tags separated by a space</param>
        /// <returns>If an empty string is passed, it will return all images.
        ///          Otherwise, it will return images that have at least one required tag</returns>
        public List<Image> FindByTags(string tagsString)
        {
            ApplicationContext db = new ApplicationContext();
            string[] requiredTags = tagsString.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (requiredTags.Length == 0)
            {
                return db.Images.ToList();
            }

            List<Image> result = new List<Image>();

            foreach (Image img in db.Images.ToList())
            {
                string[] tags = img.Tags.Split();

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
                        result.Add(img);
                        break;
                    }
                }
            }

            return result;
        }
    }
}
