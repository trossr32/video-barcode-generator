using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using FilmBarcodes.Common.Models.CafePress;
using FilmBarcodes.Common.Models.Settings;

namespace CafePress.Api
{
    public class TemplateMethods
    {
        private SettingsWrapper SettingsWrapper { get; set; }
        private ApiMethods ApiMethods { get; set; }

        public TemplateMethods(SettingsWrapper settingsWrapper)
        {
            SettingsWrapper = settingsWrapper;

            ApiMethods = new ApiMethods(SettingsWrapper);
        }

        #region count

        /// <summary>
        /// Returns the number of templates in the current user's base image folder.
        /// </summary>
        public int Count()
        {
            var resp = ApiMethods.CallApiString("template.count.cp", null, true);

            string value;

            using (StringReader reader = new StringReader(resp))
            {
                value = ((Value)new XmlSerializer(typeof(Value)).Deserialize(reader)).Text;
            }

            return int.Parse(value);
        }

        /// <summary>
        /// Returns the number images in the base image folder which contain the given creator string. 
        /// The creator string is used by applications to identify the templates which the application has created.
        /// </summary>
        /// <param name="creator">The creator.</param>
        public int CountByCreator(string creator)
        {
            var resp = ApiMethods.CallApiString("template.count.cp", new Dictionary<string, string> { { "creator", creator } }, true);

            string value;

            using (StringReader reader = new StringReader(resp))
            {
                value = ((Value)new XmlSerializer(typeof(Value)).Deserialize(reader)).Text;
            }

            return int.Parse(value);
        }

        /// <summary>
        /// Returns the number of templates in the given subfolder of the base image folder.
        /// Returns 0 if either the folder does not exist or there are no templates in the folder.
        /// </summary>
        /// <param name="folderName">Name of the folder.</param>
        public int CountByFolder(string folderName)
        {
            var resp = ApiMethods.CallApiString("template.count.cp", new Dictionary<string, string> { { "folderName", folderName } }, true);

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
        /// The creator string is used by applications to identify the templates which the application has created.
        /// </summary>
        /// <param name="creator">The creator.</param>
        /// <param name="folderName">Name of the folder.</param>
        public int CountByCreatorAndFolder(string creator, string folderName)
        {
            var resp = ApiMethods.CallApiString("template.count.cp", new Dictionary<string, string> { { "creator", creator }, { "folderName", folderName } }, true);

            string value;

            using (StringReader reader = new StringReader(resp))
            {
                value = ((Value)new XmlSerializer(typeof(Value)).Deserialize(reader)).Text;
            }

            return int.Parse(value);
        }

        /// <summary>
        /// Counts the templates in store.
        /// </summary>
        /// <param name="storeId">The store identifier.</param>
        public int CountDesignsInStore(string storeId)
        {
            var resp = ApiMethods.CallApiString("template.countDesignsInStore.cp", new Dictionary<string, string> { { "storeId", storeId } }, true);

            string value;

            using (StringReader reader = new StringReader(resp))
            {
                value = ((Value)new XmlSerializer(typeof(Value)).Deserialize(reader)).Text;
            }

            return int.Parse(value);
        }

        #endregion

        /// <summary>
        /// Create an SVG that embeds an external image.
        /// </summary>
        /// <param name="url">The URL.</param>
        public void CreateSvg(string url)
        {
             ApiMethods.Post("template.createSvg.cp", new Dictionary<string, string>{{"url", url}});
        }

        #region find

        /// <summary>
        /// Returns the template design specified by the given design id. The design must belong to the current user.
        /// </summary>
        /// <param name="id">The numeric id of a particular product.</param>
        /// <returns></returns>
        public Design Find(int id)
        {
            var resp = ApiMethods.CallApiString("template.find.cp", new Dictionary<string, string> { { "id", id.ToString() } });

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
        public Design FindFolder(int id)
        {
            var resp = ApiMethods.CallApiString("template.findFolder.cp", new Dictionary<string, string> { { "id", id.ToString() } }, true);

            using (StringReader reader = new StringReader(resp))
            {
                return (Design)new XmlSerializer(typeof(Design)).Deserialize(reader);
            }
        }

        /// <summary>
        /// Returns the SVG XML of the given template id. You may only access the SVG of templates owned by the user indicated by the user token.
        /// </summary>
        /// <param name="id">The template id.</param>
        /// <returns></returns>
        public void FindSvg(int id)
        {
            var resp = ApiMethods.CallApiString("template.findFolderByName.cp", new Dictionary<string, string> { { "id", id.ToString() } }, true);

            // resp is an svg
        }

        #endregion

        #region list and get

        /// <summary>
        /// Returns all templates in the current user's base image folder.
        /// </summary>
        /// <returns></returns>
        public Designs List()
        {
            var resp = ApiMethods.CallApiString("template.list.cp");

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
            var resp = ApiMethods.CallApiString("template.list.cp", new Dictionary<string, string> { { "folderName", folderName } });

            using (StringReader reader = new StringReader(resp))
            {
                return (Designs)new XmlSerializer(typeof(Designs)).Deserialize(reader);
            }
        }

        /// <summary>
        /// Retrieves the top 256 images in the base image folder which contain the given creator string.
        /// The creator string is used by applications to identify the templates which the application has created.
        /// </summary>
        /// <param name="creator">The creator.</param>
        /// <returns></returns>
        public Designs ListByCreator(string creator)
        {
            var resp = ApiMethods.CallApiString("template.list.cp", new Dictionary<string, string> { { "creator", creator } });

            using (StringReader reader = new StringReader(resp))
            {
                return (Designs)new XmlSerializer(typeof(Designs)).Deserialize(reader);
            }
        }

        /// <summary>
        /// Retrieves the top 256 images in the given subfolder of the base image folder which contain the given creator string. 
        /// Returns an empty list if either the folder does not exist or no files with the given creator string exist. 
        /// The creator string is used by applications to identify the templates which the application has created.
        /// </summary>
        /// <param name="folderName">Name of the folder.</param>
        /// <param name="creator">The creator.</param>
        /// <returns></returns>
        public Designs ListByFolderNameAndCreator(string folderName, string creator)
        {
            var resp = ApiMethods.CallApiString("template.list.cp", new Dictionary<string, string> { { "folderName", folderName }, { "creator", creator } });

            using (StringReader reader = new StringReader(resp))
            {
                return (Designs)new XmlSerializer(typeof(Designs)).Deserialize(reader);
            }
        }
        
        /// <summary>
        /// List all subsfolders of the base image folder, returning the name of each folder.
        /// </summary>
        /// <returns></returns>
        public void ListFolders()
        {
            var resp = ApiMethods.CallApiString("template.listFolders.cp");

            // returns list
        }

        /// <summary>
        /// Retrieves a subset of templates in the base image folder.
        /// Returns an empty list if either the folder does not exist or no files with the given creator string exist.
        /// Uses a page index and images per page to limit which images are returned.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        public void ListPage(int page, int pageSize)
        {
            var resp = ApiMethods.CallApiString("template.listPage.cp", new Dictionary<string, string> {{"page", page.ToString()}, {"pageSize", pageSize.ToString()}});

            // returns list
        }

        /// <summary>
        /// Retrieves a subset of templates in the given subfolder of the base image folder. 
        /// Returns an empty list if either the folder does not exist or no files with the given creator string exist. 
        /// Uses a zero-based page index and images per page to limit which images are returned.
        /// </summary>
        /// <param name="folderName">Name of the folder.</param>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        public void ListPage(string folderName, int page, int pageSize)
        {
            var resp = ApiMethods.CallApiString("template.listPage.cp",
                new Dictionary<string, string>
                {
                    {"folderName", folderName},
                    {"page", page.ToString()},
                    {"pageSize", pageSize.ToString()}
                });

            // returns list
        }

        /// <summary>
        /// Retrieves a subset of templates in the given subfolder of the base image folder with the given creator string.
        /// Returns an empty list if either the folder does not exist or no files with the given creator string exist.
        /// Uses a page index and images per page to limit which images are returned.
        /// </summary>
        /// <param name="folderName">Name of the folder.</param>
        /// <param name="creator">The creator.</param>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        public void ListPage(string folderName, string creator, int page, int pageSize)
        {
            var resp = ApiMethods.CallApiString("template.listPage.cp",
                new Dictionary<string, string>
                {
                    {"folderName", folderName},
                    {"creator", creator},
                    {"page", page.ToString()},
                    {"pageSize", pageSize.ToString()}
                });

            // returns list
        }

        /// <summary>
        /// Returns the last template added to the base image folder.
        /// </summary>
        /// <returns></returns>
        public Design GetLast()
        {
            var resp = ApiMethods.CallApiString("template.getLast.cp");

            using (StringReader reader = new StringReader(resp))
            {
                return (Design)new XmlSerializer(typeof(Design)).Deserialize(reader);
            }
        }

        /// <summary>
        /// Returns the last template added to the given image folder.
        /// </summary>
        /// <param name="folderName">Name of the folder.</param>
        /// <returns></returns>
        public Design GetLast(string folderName)
        {
            var resp = ApiMethods.CallApiString("template.getLast.cp", new Dictionary<string, string> { { "folderName", folderName } });

            using (StringReader reader = new StringReader(resp))
            {
                return (Design)new XmlSerializer(typeof(Design)).Deserialize(reader);
            }
        }

        #endregion
    }
}