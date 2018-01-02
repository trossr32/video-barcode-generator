using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using FilmBarcodes.Common.Helpers;
using FilmBarcodes.Common.Models.CafePress;
using FilmBarcodes.Common.Models.Settings;

namespace CafePress.Api
{
    public class DesignMethods
    {
        private SettingsWrapper SettingsWrapper { get; set; }
        private ApiMethods ApiMethods { get; set; }

        public DesignMethods(SettingsWrapper settingsWrapper)
        {
            SettingsWrapper = settingsWrapper;

            ApiMethods = new ApiMethods(SettingsWrapper);
        }

        #region count

        /// <summary>
        /// Returns the number of designs in the current user's base image folder.
        /// </summary>
        public int Count()
        {
            var resp = ApiMethods.CallApiString("design.count.cp", null, true);

            string value;

            using (StringReader reader = new StringReader(resp))
            {
                value = ((Value)new XmlSerializer(typeof(Value)).Deserialize(reader)).Text;
            }

            return int.Parse(value);
        }

        /// <summary>
        /// Returns the number images in the base image folder which contain the given creator string. 
        /// The creator string is used by applications to identify the designs which the application has created.
        /// </summary>
        /// <param name="creator">The creator.</param>
        public int CountByCreator(string creator)
        {
            var resp = ApiMethods.CallApiString("design.count.cp", new Dictionary<string, string> { { "creator", creator } }, true);

            string value;

            using (StringReader reader = new StringReader(resp))
            {
                value = ((Value)new XmlSerializer(typeof(Value)).Deserialize(reader)).Text;
            }

            return int.Parse(value);
        }

        /// <summary>
        /// Returns the number of designs in the given subfolder of the base image folder.
        /// Returns 0 if either the folder does not exist or there are no designs in the folder.
        /// </summary>
        /// <param name="folderName">Name of the folder.</param>
        public int CountByFolder(string folderName)
        {
            var resp = ApiMethods.CallApiString("design.count.cp", new Dictionary<string, string> { { "folderName", folderName } }, true);

            string value;

            using (StringReader reader = new StringReader(resp))
            {
                value = ((Value)new XmlSerializer(typeof(Value)).Deserialize(reader)).Text;
            }

            return int.Parse(value);
        }

        /// <summary>
        /// Returns the number of images in the given subfolder of the base image folder which contain the given creator string. 
        /// Returns 0 if either the folder does not exist or no files with the given creator string exist. 
        /// The creator string is used by applications to identify the designs which the application has created.
        /// </summary>
        /// <param name="creator">The creator.</param>
        /// <param name="folderName">Name of the folder.</param>
        public int CountByCreatorAndFolder(string creator, string folderName)
        {
            var resp = ApiMethods.CallApiString("design.count.cp", new Dictionary<string, string> { { "creator", creator }, { "folderName", folderName } }, true);

            string value;

            using (StringReader reader = new StringReader(resp))
            {
                value = ((Value)new XmlSerializer(typeof(Value)).Deserialize(reader)).Text;
            }

            return int.Parse(value);
        }

        /// <summary>
        /// Counts the designs in store.
        /// </summary>
        /// <param name="storeId">The store identifier.</param>
        public int CountDesignsInStore(string storeId)
        {
            var resp = ApiMethods.CallApiString("design.countDesignsInStore.cp", new Dictionary<string, string> { { "storeId", storeId } });

            string value;

            using (StringReader reader = new StringReader(resp))
            {
                value = ((Value)new XmlSerializer(typeof(Value)).Deserialize(reader)).Text;
            }

            return int.Parse(value);
        }

        #endregion

        #region find

        /// <summary>
        /// Returns the design specified by the given design id.
        /// </summary>
        /// <param name="id">The numeric id of a particular product.</param>
        /// <returns></returns>
        public Design Find(int id)
        {
            var resp = ApiMethods.CallApiString("design.find.cp", new Dictionary<string, string> { { "id", id.ToString() } }, true);

            using (StringReader reader = new StringReader(resp))
            {
                return (Design)new XmlSerializer(typeof(Design)).Deserialize(reader);
            }
        }

        /// <summary>
        /// Find the folder with the given id.
        /// </summary>
        /// <param name="id">The numeric id of a particular folder.</param>
        /// <returns></returns>
        public Folder FindFolder(int id)
        {
            var resp = ApiMethods.CallApiString("design.findFolder.cp", new Dictionary<string, string> { { "id", id.ToString() } }, true);

            using (StringReader reader = new StringReader(resp))
            {
                return (Folder)new XmlSerializer(typeof(Folder)).Deserialize(reader);
            }
        }

        /// <summary>
        /// Find the folder with the given name.
        /// </summary>
        /// <param name="name">The name of the folder to find.</param>
        /// <returns></returns>
        public Folder FindFolderByName(string name)
        {
            var resp = ApiMethods.CallApiString("design.findFolderByName.cp", new Dictionary<string, string> { { "name", name } }, true);

            using (StringReader reader = new StringReader(resp))
            {
                return (Folder)new XmlSerializer(typeof(Folder)).Deserialize(reader);
            }
        }

        /// <summary>
        /// Returns the SVG XML of the given design id. You may only access the SVG of designs owned by the user indicated by the user token.
        /// </summary>
        /// <param name="id">The design id.</param>
        /// <returns></returns>
        public void FindSvg(int id)
        {
            var resp = ApiMethods.CallApiString("design.findFolderByName.cp", new Dictionary<string, string> { { "id", id.ToString() } }, true);

            // resp is an svg
        }

        #endregion

        #region list and get

        /// <summary>
        /// Returns all designs in the current user's base image folder.
        /// </summary>
        /// <returns></returns>
        public Designs List()
        {
            var resp = ApiMethods.CallApiString("design.list.cp", null, true);

            using (StringReader reader = new StringReader(resp))
            {
                return (Designs)new XmlSerializer(typeof(Designs)).Deserialize(reader);
            }
        }

        /// <summary>
        /// Returns up to the first 256 images in the given subfolder of the base image folder. 
        /// Returns an empty list if either the folder does not exist.
        /// </summary>
        /// <param name="folderName">Name of the folder.</param>
        /// <returns></returns>
        public Designs ListByFolder(string folderName)
        {
            var resp = ApiMethods.CallApiString("design.list.cp", new Dictionary<string, string> { { "folderName", folderName } }, true);

            using (StringReader reader = new StringReader(resp))
            {
                return (Designs)new XmlSerializer(typeof(Designs)).Deserialize(reader);
            }
        }

        /// <summary>
        /// Retrieves the top 256 images in the base image folder which contain the given creator string.
        /// The creator string is used by applications to identify the designs which the application has created.
        /// </summary>
        /// <param name="creator">The creator.</param>
        /// <returns></returns>
        public Designs ListByCreator(string creator)
        {
            var resp = ApiMethods.CallApiString("design.list.cp", new Dictionary<string, string> { { "creator", creator } }, true);

            using (StringReader reader = new StringReader(resp))
            {
                return (Designs)new XmlSerializer(typeof(Designs)).Deserialize(reader);
            }
        }

        /// <summary>
        /// Retrieves the top 256 images in the given subfolder of the base image folder which contain the given creator string. 
        /// Returns an empty list if either the folder does not exist or no files with the given creator string exist. 
        /// The creator string is used by applications to identify the designs which the application has created.
        /// </summary>
        /// <param name="folderName">Name of the folder.</param>
        /// <param name="creator">The creator.</param>
        /// <returns></returns>
        public Designs ListByFolderNameAndCreator(string folderName, string creator)
        {
            var resp = ApiMethods.CallApiString("design.list.cp", new Dictionary<string, string> { { "folderName", folderName }, { "creator", creator } }, true);

            using (StringReader reader = new StringReader(resp))
            {
                return (Designs)new XmlSerializer(typeof(Designs)).Deserialize(reader);
            }
        }

        /// <summary>
        /// Lists the design categories under which a design may be categorized. 
        /// The results return pairs, the key to pass to other API methods, and the text that can be displayed to the user.
        /// </summary>
        /// <returns></returns>
        public void ListCategories()
        {
            var resp = ApiMethods.CallApiString("design.listCategories.cp");

            // returns key value pairs
        }

        /// <summary>
        /// List all subsfolders of the base image folder, returning the name of each folder.
        /// </summary>
        /// <returns></returns>
        public Folders ListFolders()
        {
            var resp = ApiMethods.CallApiString("design.listFolders.cp", null, true);

            using (StringReader reader = new StringReader(resp))
            {
                return (Folders)new XmlSerializer(typeof(Folders)).Deserialize(reader);
            }
        }

        /// <summary>
        /// Retrieves a subset of designs in the base image folder.
        /// Returns an empty list if either the folder does not exist or no files with the given creator string exist.
        /// Uses a page index and images per page to limit which images are returned.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        public void ListPage(int page, int pageSize)
        {
            var resp = ApiMethods.CallApiString("design.listPage.cp", new Dictionary<string, string> {{"page", page.ToString()}, {"pageSize", pageSize.ToString()}}, true);

            // returns list
        }

        /// <summary>
        /// Retrieves a subset of designs in the given subfolder of the base image folder. 
        /// Returns an empty list if either the folder does not exist or no files with the given creator string exist. 
        /// Uses a zero-based page index and images per page to limit which images are returned.
        /// </summary>
        /// <param name="folderName">Name of the folder.</param>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        public void ListPage(string folderName, int page, int pageSize)
        {
            var resp = ApiMethods.CallApiString("design.listPage.cp",
                new Dictionary<string, string>
                {
                    {"folderName", folderName},
                    {"page", page.ToString()},
                    {"pageSize", pageSize.ToString()}
                }, true);

            // returns list
        }

        /// <summary>
        /// Retrieves a subset of designs in the given subfolder of the base image folder with the given creator string.
        /// Returns an empty list if either the folder does not exist or no files with the given creator string exist.
        /// Uses a page index and images per page to limit which images are returned.
        /// </summary>
        /// <param name="folderName">Name of the folder.</param>
        /// <param name="creator">The creator.</param>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        public void ListPage(string folderName, string creator, int page, int pageSize)
        {
            var resp = ApiMethods.CallApiString("design.listPage.cp",
                new Dictionary<string, string>
                {
                    {"folderName", folderName},
                    {"creator", creator},
                    {"page", page.ToString()},
                    {"pageSize", pageSize.ToString()}
                }, true);

            // returns list
        }

        /// <summary>
        /// Returns the last design added to the base image folder.
        /// </summary>
        /// <returns></returns>
        public Design GetLast()
        {
            var resp = ApiMethods.CallApiString("design.getLast.cp", null, true);

            using (StringReader reader = new StringReader(resp))
            {
                return (Design)new XmlSerializer(typeof(Design)).Deserialize(reader);
            }
        }

        /// <summary>
        /// Returns the last design added to the given image folder.
        /// </summary>
        /// <param name="folderName">Name of the folder.</param>
        /// <returns></returns>
        public Design GetLast(string folderName)
        {
            var resp = ApiMethods.CallApiString("design.getLast.cp", new Dictionary<string, string> { { "folderName", folderName } }, true);

            using (StringReader reader = new StringReader(resp))
            {
                return (Design)new XmlSerializer(typeof(Design)).Deserialize(reader);
            }
        }

        #endregion

        #region move and tag

        /// <summary>
        /// Moves designs of the given ids to the specified folder and categorizes them with the given category.
        /// Design ids may represent uploaded raster images or saved SVG designs.
        /// </summary>
        /// <param name="designIds">The design ids.</param>
        /// <param name="folderName">Name of the folder.</param>
        /// <param name="category">The category.</param>
        public void MoveAndCategorizeDesigns(string designIds, string folderName, string category)
        {
            var resp = ApiMethods.CallApiString("design.moveAndCategorizeDesigns.cp",
                new Dictionary<string, string>
                {
                    {"designIds", designIds},
                    {"folderName", folderName},
                    {"category", category}
                }, true);
        }

        /// <summary>
        /// Moves designs of the given ids to the specified folder and tags them with the given tags. 
        /// Design ids may represent uploaded raster images or saved SVG designs.
        /// </summary>
        /// <param name="designIds">The design ids.</param>
        /// <param name="folderName">Name of the folder.</param>
        /// <param name="tags">The tags.</param>
        public void MoveAndTagDesigns(string designIds, string folderName, string tags)
        {
            var resp = ApiMethods.CallApiString("design.moveAndTagDesigns.cp",
                new Dictionary<string, string>
                {
                    {"designIds", designIds},
                    {"folderName", folderName},
                    {"tags", tags}
                }, true);
        }

        /// <summary>
        /// Moves designs of the given ids to the specified folder, tags them with the given tags, and categorizes them with the given category. 
        /// Design ids may represent uploaded raster images or saved SVG designs.
        /// </summary>
        /// <param name="designIds">The design ids.</param>
        /// <param name="folderName">Name of the folder.</param>
        /// <param name="tags">The tags.</param>
        /// <param name="category">The category.</param>
        public void MoveAndTagDesigns(string designIds, string folderName, string tags, string category)
        {
            var resp = ApiMethods.CallApiString("design.moveAndTagDesigns.cp",
                new Dictionary<string, string>
                {
                    {"designIds", designIds},
                    {"folderName", folderName},
                    {"tags", tags},
                    {"category", category}
                }, true);
        }

        /// <summary>
        /// Moves designs of the given ids to the specified folder. 
        /// Design ids may represent uploaded raster images or saved SVG designs.
        /// </summary>
        /// <param name="designIds">The design ids.</param>
        /// <param name="folderName">Name of the folder.</param>
        public void MoveDesigns(string designIds, string folderName)
        {
            var resp = ApiMethods.CallApiString("design.moveDesigns.cp",
                new Dictionary<string, string>
                {
                    {"designIds", designIds},
                    {"folderName", folderName}
                }, true);
        }

        /// <summary>
        /// Tags design of the given ids with the given tags. Design ids may represent uploaded raster images or saved SVG designs.
        /// </summary>
        /// <param name="designIds">The design ids.</param>
        /// <param name="tags">The tags.</param>
        public void TagDesigns(string designIds, string tags)
        {
            var resp = ApiMethods.CallApiString("design.tagDesigns.cp",
                new Dictionary<string, string>
                {
                    {"designIds", designIds},
                    {"tags", tags}
                }, true);
        }

        /// <summary>
        /// Tags design of the given ids with the given tags. Design ids may represent uploaded raster images or saved SVG designs.
        /// </summary>
        /// <param name="designIds">The design ids.</param>
        /// <param name="tags">The tags.</param>
        /// <param name="category">The category.</param>
        public void TagDesigns(string designIds, string tags, string category)
        {
            var resp = ApiMethods.CallApiString("design.tagDesigns.cp",
                new Dictionary<string, string>
                {
                    {"designIds", designIds},
                    {"tags", tags},
                    {"category", category}
                }, true);
        }

        #endregion

        #region save, update and make

        /// <summary>
        /// Creates a subfolder of the base image folder with the given name. 
        /// If the folder already exists no folder is created. In either case folder name is returned.
        /// </summary>
        /// <param name="folderName">Name of the folder.</param>
        /// <returns></returns>
        public void MakeFolder(string folderName)
        {
            var resp = ApiMethods.CallApiString("design.makeFolder.cp", new Dictionary<string, string> { { "folderName", folderName } }, true);
        }

        /// <summary>
        /// Saves a new design with the given SVG XML specified outside of the design definition.
        /// If the design does not specify a parent folder, it is stored or moved to the base image folder.
        /// Note that you can no longer update the SVG XML of an existing design.
        /// If you call this method with an existing design, a new design will be created with the given design, SVG, and folder properties.
        /// </summary>
        /// <param name="design">The design.</param>
        /// <param name="svg">The SVG.</param>
        public void Save(Design design, string svg)
        {
             ApiMethods.Post("design.save.cp", new Dictionary<string, string> { { "svg", svg } }, design.Serialize(), true);
        }

        /// <summary>
        /// Saves a the new design with the given SVG XML specified outside of the design definition. 
        /// The design is created in the given subfolder of the base image folder. 
        /// Note that you can no longer update the SVG XML of an existing design. 
        /// If you call this method with an existing design, a new design will be created with the given design, SVG, and folder properties.
        /// </summary>
        /// <param name="design">The design.</param>
        /// <param name="svg">The SVG.</param>
        /// <param name="folderName">Name of the folder.</param>
        public void Save(Design design, string svg, string folderName)
        {
             ApiMethods.Post("design.save.cp", new Dictionary<string, string> { { "svg", svg }, { "folderName", folderName } }, design.Serialize(), true);
        }

        /// <summary>
        /// Save design family for given image number.
        /// </summary>
        /// <param name="imageNo">The image no.</param>
        /// <param name="designFamilyName">Name of the design family.</param>
        /// <param name="htmlColorHexValue">The HTML color hexadecimal value.</param>
        public void SaveDesignFamily(string imageNo, string designFamilyName, string htmlColorHexValue)
        {
             ApiMethods.Post("design.saveDesignFamily.cp",
                new Dictionary<string, string>
                {
                    {"imageNo", imageNo},
                    {"designFamilyName", designFamilyName},
                    {"htmlColorHexValue", htmlColorHexValue}
                }, "", true);
        }

        /// <summary>
        /// Updates the given design without updating its SVG XML. Existing designs have a non-zero id. 
        /// If the design does not specify a parent folder, it is moved to the base image folder. 
        /// The design must belong to the current user.
        /// </summary>
        /// <param name="design">The design.</param>
        public void Update(Design design)
        {
             ApiMethods.Post("design.update.cp", null, design.Serialize(), true);
        }

        #endregion
    }
}