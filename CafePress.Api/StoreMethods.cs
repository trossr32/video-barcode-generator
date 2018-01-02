using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using FilmBarcodes.Common.Helpers;
using FilmBarcodes.Common.Models.CafePress;
using FilmBarcodes.Common.Models.Settings;

namespace CafePress.Api
{
    public class StoreMethods
    {
        private SettingsWrapper SettingsWrapper { get; set; }
        private ApiMethods ApiMethods { get; set; }

        public StoreMethods(SettingsWrapper settingsWrapper)
        {
            SettingsWrapper = settingsWrapper;

            ApiMethods = new ApiMethods(SettingsWrapper);
        }

        #region list

        /// <summary>
        /// Lists all the stores of the current user.
        /// </summary>
        /// <returns></returns>
        public Stores ListStores()
        {
            var resp = ApiMethods.CallApiString("store.listStores.cp", null, true);

            using (StringReader reader = new StringReader(resp))
            {
                return (Stores)new XmlSerializer(typeof(Stores)).Deserialize(reader);
            }
        }

        /// <summary>
        /// Lists the top-level sections of the given store.
        /// </summary>
        /// <param name="storeId">The store identifier.</param>
        /// <returns></returns>
        public StoreSections ListStoreSections(string storeId)
        {
            var resp = ApiMethods.CallApiString("store.listStoreSections.cp",
                new Dictionary<string, string> { { "storeId", storeId } });

            using (StringReader reader = new StringReader(resp))
            {
                return (StoreSections)new XmlSerializer(typeof(StoreSections)).Deserialize(reader);
            }
        }

        /// <summary>
        /// Lists the store sections of the given store and store section.
        /// </summary>
        /// <param name="storeId">The store identifier.</param>
        /// <param name="storeSectionId">The store section identifier.</param>
        /// <returns></returns>
        public StoreSections ListStoreSubSections(string storeId, string storeSectionId)
        {
            var resp = ApiMethods.CallApiString("store.listStoreSubSections.cp",
                new Dictionary<string, string> { { "storeId", storeId }, { "storeSectionId", storeSectionId } });

            using (StringReader reader = new StringReader(resp))
            {
                return (StoreSections)new XmlSerializer(typeof(StoreSections)).Deserialize(reader);
            }
        }

        #endregion

        #region save

        /// <summary>
        /// Creates or updates the given store. Identify a store with the string id which is the URL path of the store. 
        /// Use IsStoreIdTaken() to verify a store id is available before creating a new store. 
        /// When updating an existing store, it must belong to the current user.
        /// </summary>
        /// <param name="store">The store.</param>
        public void SaveStore(Store store)
        {
             ApiMethods.Post("store.saveStore.cp", null, store.Serialize(), true);
        }

        /// <summary>
        /// Creates or updates the given store section. Identify a store section with the numeric id, or use 0 to indicate a new section. 
        /// You cannot update the base section of a store (which is identified in find methods by id=0). 
        /// When updating an existing section, it must belong to the current user.
        /// </summary>
        /// <param name="storeSection">The store section.</param>
        public void SaveStoreSection(StoreSection storeSection)
        {
             ApiMethods.Post("store.saveStoreSection.cp", null, storeSection.Serialize(), true);
        }

        #endregion

        #region count

        /// <summary>
        /// Counts the top-level sections of the given store.
        /// </summary>
        /// <param name="storeId">The store identifier.</param>
        /// <returns></returns>
        public int CountStoreSections(string storeId)
        {
            var resp = ApiMethods.CallApiString("store.countStoreSections.cp", new Dictionary<string, string> { { "storeId", storeId } });

            string value;

            using (StringReader reader = new StringReader(resp))
            {
                value = ((Value)new XmlSerializer(typeof(Value)).Deserialize(reader)).Text;
            }

            return int.TryParse(value, out int i) ? i : 0;
        }

        /// <summary>
        /// Counts the store sections of the given store and store section.
        /// </summary>
        /// <param name="storeId">The store identifier.</param>
        /// <param name="storeSectionId">The store section identifier.</param>
        /// <returns></returns>
        public int CountStoreSubSections(string storeId, string storeSectionId)
        {
            var resp = ApiMethods.CallApiString("store.countStoreSubSections.cp", new Dictionary<string, string> { { "storeId", storeId }, { "storeSectionId", storeSectionId } });

            string value;

            using (StringReader reader = new StringReader(resp))
            {
                value = ((Value)new XmlSerializer(typeof(Value)).Deserialize(reader)).Text;
            }

            return int.TryParse(value, out int i) ? i : 0;
        }

        #endregion

        #region find

        /// <summary>
        /// Returns the store in which the product resides. The product may belong to any user.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns></returns>
        public Stores FindByProductId(string productId)
        {
            var resp = ApiMethods.CallApiString("store.findByProductId.cp", new Dictionary<string, string> { { "productId", productId } });

            using (StringReader reader = new StringReader(resp))
            {
                return (Stores)new XmlSerializer(typeof(Stores)).Deserialize(reader);
            }
        }

        /// <summary>
        /// Returns the store with the given id allowing the user to look up a disabled store.
        /// The specified store must belong to the current user.
        /// </summary>
        /// <param name="storeId">The store identifier.</param>
        /// <param name="disabled">if set to <c>true</c> [disabled].</param>
        /// <returns></returns>
        public Stores FindByStoreId(string storeId, bool disabled)
        {
            var resp = ApiMethods.CallApiString("store.findByStoreId.cp", new Dictionary<string, string> { { "storeId", storeId }, { "disabled", disabled.ToString() } });

            using (StringReader reader = new StringReader(resp))
            {
                return (Stores)new XmlSerializer(typeof(Stores)).Deserialize(reader);
            }
        }

        /// <summary>
        /// Returns the store with the given id.
        /// </summary>
        /// <param name="storeId">The store identifier.</param>
        /// <returns></returns>
        public Stores FindByStoreId(string storeId)
        {
            var resp = ApiMethods.CallApiString("store.findByStoreId.cp", new Dictionary<string, string> { { "storeId", storeId } });

            using (StringReader reader = new StringReader(resp))
            {
                return (Stores)new XmlSerializer(typeof(Stores)).Deserialize(reader);
            }
        }

        /// <summary>
        /// Returns the store section with the given store and section id. The specified store must belong to the current user.
        /// </summary>
        /// <param name="storeId">The store identifier.</param>
        /// <param name="storeSectionId">The store section identifier.</param>
        /// <returns></returns>
        public Stores FindByStoreSectionId(string storeId, string storeSectionId)
        {
            var resp = ApiMethods.CallApiString("store.findByStoreSectionId.cp", new Dictionary<string, string> { { "storeId", storeId }, {"storeSectionId", storeSectionId} });

            using (StringReader reader = new StringReader(resp))
            {
                return (Stores)new XmlSerializer(typeof(Stores)).Deserialize(reader);
            }
        }

        #endregion

        /// <summary>
        /// Checks to see if the given storeId is already taken by any user.
        /// </summary>
        /// <param name="storeId">The store identifier.</param>
        /// <returns>
        ///   <c>true</c> if [is store identifier taken] [the specified store identifier]; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.Exception"></exception>
        public bool IsStoreIdTaken(string storeId)
        {
            var resp = ApiMethods.CallApiString("store.isStoreIdTaken.cp", new Dictionary<string, string> { { "storeId", storeId } });

            string value;

            using (StringReader reader = new StringReader(resp))
            {
                value = ((Value)new XmlSerializer(typeof(Value)).Deserialize(reader)).Text;
            }

            return bool.TryParse(value, out bool i) ? i : true;
        }
    }
}