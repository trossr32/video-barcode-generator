using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using FilmBarcodes.Common.Helpers;
using FilmBarcodes.Common.Models.CafePress;

namespace CafePress.Api
{
    public class ProductMethods
    {
        private SettingsWrapper SettingsWrapper { get; set; }
        private ApiMethods ApiMethods { get; set; }

        public ProductMethods(SettingsWrapper settingsWrapper)
        {
            SettingsWrapper = settingsWrapper;

            ApiMethods = new ApiMethods(SettingsWrapper);
        }

        #region list

        /// <summary>
        /// Returns all products of the given store id in the top-level store (i.e. storeSectionId=0). 
        /// The specified store must belong to the current user if it is private or not visible.
        /// </summary>
        /// <param name="storeId">The alpha-numeric id of a particular store.</param>
        /// <returns></returns>
        public Products ListByStore(string storeId)
        {
            var resp = ApiMethods.CallApiString("product.listByStore.cp",
                new Dictionary<string, string> { { "storeId", storeId } });

            using (StringReader reader = new StringReader(resp))
            {
                return (Products)new XmlSerializer(typeof(Products)).Deserialize(reader);
            }
        }

        /// <summary>
        /// Returns products of the given store id in the top-level store (i.e. storeSectionId=0) limited by the comma-separated list of merchandise ids. 
        /// The specified store must belong to the current user if it is private or not visible.
        /// </summary>
        /// <param name="storeId">The alpha-numeric id of a particular store.</param>
        /// <param name="merchandiseIds">
        ///     Ids of merchandise. Use a comma-separated list of ids or one 'merchandiseIds' parameter per id. 
        ///     Exclude merchandise by listing negative ids. If any negative ids are found the inverse set of those negatives will be returned (positive ids will be ignored if negatives occur)
        /// </param>
        /// <returns></returns>
        public Products ListByStore(string storeId, string merchandiseIds)
        {
            var resp = ApiMethods.CallApiString("product.listByStore.cp",
                new Dictionary<string, string> { { "storeId", storeId }, { "merchandiseIds", merchandiseIds } });

            using (StringReader reader = new StringReader(resp))
            {
                return (Products)new XmlSerializer(typeof(Products)).Deserialize(reader);
            }
        }

        /// <summary>
        /// Returns products of the given store id in the top-level store (i.e. storeSectionId=0) limited by the comma-separated list of merchandise ids. 
        /// The specified store must belong to the current user if it is private or not visible.
        /// </summary>
        /// <param name="storeId">The alpha-numeric id of a particular store.</param>
        /// <param name="merchandiseIds">
        ///     Ids of merchandise. Use a comma-separated list of ids or one 'merchandiseIds' parameter per id. 
        ///     Exclude merchandise by listing negative ids. If any negative ids are found the inverse set of those negatives will be returned (positive ids will be ignored if negatives occur)
        /// </param>
        /// <param name="page">A Zero-based index into the list of products.</param>
        /// <param name="pageSize">The number of products per page.</param>
        /// <returns></returns>
        public Products ListByStore(string storeId, string merchandiseIds, int page, int pageSize)
        {
            var resp = ApiMethods.CallApiString("product.listByStore.cp",
                new Dictionary<string, string>
                {
                    {"storeId", storeId},
                    {"merchandiseIds", merchandiseIds},
                    {"page", page.ToString()},
                    {"pageSize", pageSize.ToString()}
                });

            using (StringReader reader = new StringReader(resp))
            {
                return (Products)new XmlSerializer(typeof(Products)).Deserialize(reader);
            }
        }

        /// <summary>
        /// Returns all products of the given store id in the top-level store (i.e. storeSectionId=0). 
        /// The specified store must belong to the current user if it is private or not visible.
        /// </summary>
        /// <param name="storeId">The alpha-numeric id of a particular store.</param>
        /// <param name="page">A Zero-based index into the list of products.</param>
        /// <param name="pageSize">The number of products per page.</param>
        /// <returns></returns>
        public Products ListByStore(string storeId, int page, int pageSize)
        {
            var resp = ApiMethods.CallApiString("product.listByStore.cp",
                new Dictionary<string, string>
                {
                    {"storeId", storeId},
                    {"page", page.ToString()},
                    {"pageSize", pageSize.ToString()}
                });

            using (StringReader reader = new StringReader(resp))
            {
                return (Products)new XmlSerializer(typeof(Products)).Deserialize(reader);
            }
        }

        /// <summary>
        /// Returns all products of the given store and section id. 
        /// The specified store section must belong to a store of the current user if it is not visible. 
        /// Section id 0 is the root section.
        /// </summary>
        /// <param name="storeId">The alpha-numeric id of a particular store.</param>
        /// <param name="storeSectionId">The numeric id of a section of a store. Section ids are unique except for section id 0, which represents the top-level contents of a store..</param>
        /// <returns></returns>
        public Products ListByStoreSection(string storeId, string storeSectionId)
        {
            var resp = ApiMethods.CallApiString("product.listByStoreSection.cp", new Dictionary<string, string> {{"storeId", storeId}, {"storeSectionId", storeSectionId}});

            using (StringReader reader = new StringReader(resp))
            {
                return (Products)new XmlSerializer(typeof(Products)).Deserialize(reader);
            }
        }

        /// <summary>
        /// Returns products of the given store and section id limited by the comma-separated list of merchandise ids. 
        /// The specified store section must belong to a store of the current user if it is not visible. 
        /// Section id 0 is the root section.
        /// </summary>
        /// <param name="storeId">The alpha-numeric id of a particular store.</param>
        /// <param name="storeSectionId">The numeric id of a section of a store. Section ids are unique except for section id 0, which represents the top-level contents of a store..</param>
        /// <param name="merchandiseIds">
        ///     Ids of merchandise. Use a comma-separated list of ids or one 'merchandiseIds' parameter per id. 
        ///     Exclude merchandise by listing negative ids. If any negative ids are found the inverse set of those negatives will be returned (positive ids will be ignored if negatives occur)
        /// </param>
        /// <returns></returns>
        public Products ListByStoreSection(string storeId, string storeSectionId, string merchandiseIds)
        {
            var resp = ApiMethods.CallApiString("product.listByStoreSection.cp",
                new Dictionary<string, string> { { "storeId", storeId }, { "storeSectionId", storeSectionId }, { "merchandiseIds", merchandiseIds } });

            using (StringReader reader = new StringReader(resp))
            {
                return (Products)new XmlSerializer(typeof(Products)).Deserialize(reader);
            }
        }

        /// <summary>
        /// Returns products of the given store and section id limited by the comma-separated list of merchandise ids. 
        /// The specified store section must belong to a store of the current user if it is not visible. 
        /// Section id 0 is the root section.
        /// </summary>
        /// <param name="storeId">The alpha-numeric id of a particular store.</param>
        /// <param name="storeSectionId">The numeric id of a section of a store. Section ids are unique except for section id 0, which represents the top-level contents of a store..</param>
        /// <param name="merchandiseIds">
        ///     Ids of merchandise. Use a comma-separated list of ids or one 'merchandiseIds' parameter per id. 
        ///     Exclude merchandise by listing negative ids. If any negative ids are found the inverse set of those negatives will be returned (positive ids will be ignored if negatives occur)
        /// </param>
        /// <param name="page">A Zero-based index into the list of products.</param>
        /// <param name="pageSize">The number of products per page.</param>
        /// <returns></returns>
        public Products ListByStoreSection(string storeId, string storeSectionId, string merchandiseIds, int page, int pageSize)
        {
            var resp = ApiMethods.CallApiString("product.listByStoreSection.cp",
                new Dictionary<string, string>
                {
                    {"storeId", storeId},
                    {"storeSectionId", storeSectionId},
                    {"merchandiseIds", merchandiseIds},
                    {"page", page.ToString()},
                    {"pageSize", pageSize.ToString()}
                });

            using (StringReader reader = new StringReader(resp))
            {
                return (Products)new XmlSerializer(typeof(Products)).Deserialize(reader);
            }
        }

        /// <summary>
        /// Returns all products of the given store and section id. 
        /// The specified store section must belong to a store of the current user if it is not visible. 
        /// Section id 0 is the root section.
        /// </summary>
        /// <param name="storeId">The alpha-numeric id of a particular store.</param>
        /// <param name="storeSectionId">The numeric id of a section of a store. Section ids are unique except for section id 0, which represents the top-level contents of a store.</param>
        /// <param name="page">A Zero-based index into the list of products.</param>
        /// <param name="pageSize">The number of products per page.</param>
        /// <returns></returns>
        public Products ListByStoreSection(string storeId, string storeSectionId, int page, int pageSize)
        {
            var resp = ApiMethods.CallApiString("product.listByStoreSection.cp",
                new Dictionary<string, string>
                {
                    {"storeId", storeId},
                    {"storeSectionId", storeSectionId},
                    {"page", page.ToString()},
                    {"pageSize", pageSize.ToString()}
                });

            using (StringReader reader = new StringReader(resp))
            {
                return (Products)new XmlSerializer(typeof(Products)).Deserialize(reader);
            }
        }

        /// <summary>
        /// Returns all products of the given store id and all products in sections and subsections.
        /// The results are paged with a maximum of 256 results per page. 
        /// The specified store must belong to the current user (the matching the user token, if provided) if it is not visible. 
        /// Hidden sections and their subsections will be skipped unless the current user owns the store.
        /// </summary>
        /// <param name="storeId">The alpha-numeric id of a particular store.</param>
        /// <param name="page">A Zero-based index into the list of products.</param>
        /// <param name="pageSize">The number of products per page.</param>
        /// <returns></returns>
        public Products ListDeepByStore(string storeId, int page, int pageSize)
        {
            var resp = ApiMethods.CallApiString("product.listDeepByStore.cp", new Dictionary<string, string>
            {
                {"storeId", storeId},
                {"page", page.ToString()},
                {"pageSize", pageSize.ToString()}
            });

            using (StringReader reader = new StringReader(resp))
            {
                return (Products)new XmlSerializer(typeof(Products)).Deserialize(reader);
            }
        }

        /// <summary>
        /// Retuns all products created by the current anonymous user. 
        /// Anonymous users have no stores thus this is the only access to their products. The current user must be anonymous.
        /// </summary>
        /// <returns></returns>
        public Products ListAnonymousProducts(string storeId, int page, int pageSize)
        {
            var resp = ApiMethods.CallApiString("product.listAnonymousProducts.cp", null, true);

            using (StringReader reader = new StringReader(resp))
            {
                return (Products)new XmlSerializer(typeof(Products)).Deserialize(reader);
            }
        }
        #endregion

        #region save

        /// <summary>
        /// Creates or updates the given product. 
        /// Existing products have a non-zero id. The product must belong to the current user. 
        /// If product definition's user id, if omitted, will default to the current user. 
        /// The product definition's store id, if omitted, will default to the unspecified store id for registered users. 
        /// The product definition's store section id, if omitted, will cause the product to save to the base store location.
        /// </summary>
        /// <param name="product">The product.</param>
        public void Save(Product product)
        {
             ApiMethods.Post("product.save.cp", null, product.Serialize(), true);
        }

        /// <summary>
        /// Creates or updates the given product. Existing products have a non-zero id. 
        /// The product must belong to the current user. If product definition's user id, if omitted, will default to the current user. 
        /// The isActive parameter defines whether the saved product will be active. 
        /// The product definition's store id, if omitted, will default to the unspecified store id for registered users. 
        /// The product definition's store section id, if omitted, will cause the product to save to the base store location.
        /// </summary>
        /// <param name="products">The products.</param>
        /// <param name="isActive">Indicates whether the saved product will be active.</param>
        public void SaveActive(Products products, bool isActive)
        {
             ApiMethods.Post("product.saveActive.cp", null, products.Serialize(), true);
        }

        /// <summary>
        /// Creates or updates the given products. Existing products have a non-zero id. 
        /// The products must belong to the current user. Each product definition's user id, if omitted, will default to the current user. 
        /// Each product definition's store id, if omitted, will default to the unspecified store id for registered users. 
        /// Each product definition's store section id, if omitted, will cause the product to save to the base store location.
        /// </summary>
        /// <param name="products">The products.</param>
        /// <param name="isActive">Indicates whether the saved product will be active.</param>
        public void SaveAll(Products products, bool isActive)
        {
             ApiMethods.Post("product.saveAll.cp", null, products.Serialize(), true);
        }

        /// <summary>
        /// Creates or updates the given product. Existing products have a non-zero id. The product must belong to the current user.
        /// If product definition's user id, if omitted, will default to the current user.
        /// The isActive parameter defines whether the saved product will be active.
        /// The product definition's store id, if omitted, will default to the unspecified store id for registered users.
        /// The product definition's store section id, if omitted, will cause the product to save to the base store location.
        /// </summary>
        /// <param name="product">The product.</param>
        /// <param name="ezpSku">The SKU number from EZPrints Stationery Template.</param>
        /// <param name="ezpTemplateId">The Template Id number from EZPrints Stationery Template.</param>
        /// <param name="ezpTemplateName">The Template Partner Id from EZPrints Stationery Template.</param>
        public void SaveEzpBuilderTemplateProduct(Product product, string ezpSku, string ezpTemplateId, string ezpTemplateName)
        {
             ApiMethods.Post("product.saveActive.cp",
                new Dictionary<string, string>
                {
                    {"EzpSku", ezpSku},
                    {"EzpTemplateId", ezpTemplateId},
                    {"EzpTemplateName", ezpTemplateName}
                }, product.Serialize(), true, "ProductId");
        }

        /// <summary>
        /// Saves product with mediaconfiguration's height set to max of allowed height on the product type.
        /// </summary>
        /// <param name="product">The product.</param>
        public void SaveWithMaxHeight(Product product)
        {
             ApiMethods.Post("product.saveWithMaxHeight.cp", null, product.Serialize(), true);
        }

        #endregion

        #region count

        /// <summary>
        /// Counts all products of the given store id in the top-level store (i.e. storeSectionId=0).
        /// The specified store must belong to the current user if it is private or not visible.
        /// </summary>
        /// <param name="storeId">The alpha-numeric id of a particular store.</param>
        /// <returns></returns>
        public int CountByStore(string storeId)
        {
            var resp = ApiMethods.CallApiString("product.countByStore.cp", new Dictionary<string, string> { { "storeId", storeId } });

            string value;

            using (StringReader reader = new StringReader(resp))
            {
                value = ((Value) new XmlSerializer(typeof(Value)).Deserialize(reader)).Text;
            }

            return int.TryParse(value, out int i) ? i : 0;
        }

        /// <summary>
        /// Counts products of the given store id in the top-level store (i.e. storeSectionId=0) limited by the comma-separated list of merchandise ids. 
        /// The specified store must belong to the current user if it is private or not visible
        /// </summary>
        /// <param name="storeId">The alpha-numeric id of a particular store.</param>
        /// <param name="merchandiseIds">
        ///     Ids of merchandise. Use a comma-separated list of ids or one 'merchandiseIds' parameter per id. 
        ///     Exclude merchandise by listing negative ids. 
        ///     If any negative ids are found the inverse set of those negatives will be returned (positive ids will be ignored if negatives occur.)
        /// </param>
        /// <returns></returns>
        public int CountByStore(string storeId, string merchandiseIds)
        {
            var resp = ApiMethods.CallApiString("product.countByStore.cp", new Dictionary<string, string>
            {
                {"storeId", storeId},
                { "merchandiseIds", merchandiseIds}
            });

            string value;

            using (StringReader reader = new StringReader(resp))
            {
                value = ((Value)new XmlSerializer(typeof(Value)).Deserialize(reader)).Text;
            }

            return int.TryParse(value, out int i) ? i : 0;
        }

        /// <summary>
        /// Counts all products of the given store and section id. 
        /// The specified store section must belong to a store of the current user if it is not visible. 
        /// Section id 0 is the root section.
        /// </summary>
        /// <param name="storeId">The alpha-numeric id of a particular store.</param>
        /// <param name="sectionId">The numeric id of a section of a store. Section ids are unique except for section id 0, which represents the top-level contents of a store.</param>
        /// <returns></returns>
        public int CountByStoreSection(string storeId, string sectionId)
        {
            var resp = ApiMethods.CallApiString("product.countByStoreSection.cp", new Dictionary<string, string> { { "storeId", storeId }, { "sectionId", sectionId } });

            string value;

            using (StringReader reader = new StringReader(resp))
            {
                value = ((Value)new XmlSerializer(typeof(Value)).Deserialize(reader)).Text;
            }

            return int.TryParse(value, out int i) ? i : 0;
        }

        /// <summary>
        /// Counts products of the given store and section id limited by the comma-separated list of merchandise ids. 
        /// The specified store section must belong to a store of the current user if it is not visible. 
        /// Section id 0 is the root section.
        /// </summary>
        /// <param name="storeId">The alpha-numeric id of a particular store.</param>
        /// <param name="sectionId">The numeric id of a section of a store. Section ids are unique except for section id 0, which represents the top-level contents of a store.</param>
        /// <param name="merchandiseIds">
        ///     Ids of merchandise. Use a comma-separated list of ids or one 'merchandiseIds' parameter per id. 
        ///     Exclude merchandise by listing negative ids. 
        ///     If any negative ids are found the inverse set of those negatives will be returned (positive ids will be ignored if negatives occur.)
        /// </param>
        /// <returns></returns>
        public int CountByStoreSection(string storeId, string sectionId, string merchandiseIds)
        {
            var resp = ApiMethods.CallApiString("product.countByStoreSection.cp", new Dictionary<string, string>
            {
                {"storeId", storeId},
                {"sectionId", sectionId},
                {"merchandiseIds", merchandiseIds}
            });

            string value;

            using (StringReader reader = new StringReader(resp))
            {
                value = ((Value)new XmlSerializer(typeof(Value)).Deserialize(reader)).Text;
            }

            return int.TryParse(value, out int i) ? i : 0;
        }

        /// <summary>
        /// Returns a count of all products of the given store id and all products in sections and subsections. 
        /// The specified store must belong to the current user (the matching the user token, if provided) if it is not visible. 
        /// Hidden sections and their subsections will be skipped unless the current user owns the store.
        /// </summary>
        /// <param name="storeId">The alpha-numeric id of a particular store.</param>
        /// <returns></returns>
        public int CountDeepByStore(string storeId)
        {
            var resp = ApiMethods.CallApiString("product.countDeepByStore.cp", new Dictionary<string, string> { { "storeId", storeId } });

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
        /// Returns the product with the given id.
        /// </summary>
        /// <param name="id">The numeric id of a particular product.</param>
        /// <returns></returns>
        public Product Find(int id)
        {
            var resp = ApiMethods.CallApiString("product.find.cp", new Dictionary<string, string> { { "id", id.ToString() } });

            using (StringReader reader = new StringReader(resp))
            {
                return (Product)new XmlSerializer(typeof(Product)).Deserialize(reader);
            }
        }

        /// <summary>
        /// 	Returns the all sellable products with the given designId. Incomplete products are excluded.
        /// </summary>
        /// <param name="designId">The design identifier.</param>
        /// <returns></returns>
        public Products FindByDesignId(int designId)
        {
            var resp = ApiMethods.CallApiString("product.findByDesignId.cp", new Dictionary<string, string> { { "designId", designId.ToString() } });

            using (StringReader reader = new StringReader(resp))
            {
                return (Products)new XmlSerializer(typeof(Products)).Deserialize(reader);
            }
        }

        /// <summary>
        /// Get ezp template product by template id.
        /// </summary>
        /// <param name="ezpTemplateId">The ezp template identifier.</param>
        /// <returns></returns>
        public int FindEzpBuilderTemplateByTemplateId(int ezpTemplateId)
        {
            var resp = ApiMethods.CallApiString("product.findEzpBuilderTemplateByTemplateId.cp", new Dictionary<string, string> { { "EzpTemplateId", ezpTemplateId.ToString() } });

            string value;

            using (StringReader reader = new StringReader(resp))
            {
                value = ((Value)new XmlSerializer(typeof(Value)).Deserialize(reader)).Text;
            }

            return int.TryParse(value, out int i) ? i : 0;
        }

        #endregion

        #region search

        /// <summary>
        /// Search for products among all merchandise types matching the given criteria.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="resultsPerPage">The results per page.</param>
        /// <param name="maxProductsPerDesign">The maximum products per design.</param>
        /// <param name="sort">The sort.</param>
        /// <returns></returns>
        public Products Search(string query, int pageNumber, int resultsPerPage, int maxProductsPerDesign, string sort)
        {
            var resp = ApiMethods.CallApiString("product.search.cp",
                new Dictionary<string, string>
                {
                    {"query", query},
                    {"pageNumber", pageNumber.ToString()},
                    {"resultsPerPage", resultsPerPage.ToString()},
                    {"maxProductsPerDesign", maxProductsPerDesign.ToString()},
                    {"sort", sort}
                });

            using (StringReader reader = new StringReader(resp))
            {
                return (Products)new XmlSerializer(typeof(Products)).Deserialize(reader);
            }
        }

        /// <summary>
        /// Search for products of certain merchandise ids matching the given criteria.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="merchandiseIds">
        ///     Ids of merchandise. Use a comma-separated list of ids or one 'merchandiseIds' parameter per id. 
        ///     Exclude merchandise by listing negative ids. 
        ///     If any negative ids are found the inverse set of those negatives will be returned (positive ids will be ignored if negatives occur.)
        /// </param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="resultsPerPage">The results per page.</param>
        /// <param name="maxProductsPerDesign">The maximum products per design.</param>
        /// <param name="sort">The sort.</param>
        /// <returns></returns>
        public Products Search(string query, string merchandiseIds, int pageNumber, int resultsPerPage, int maxProductsPerDesign, string sort)
        {
            var resp = ApiMethods.CallApiString("product.search.cp", new Dictionary<string, string> { {"merchandiseIds", merchandiseIds} });

            using (StringReader reader = new StringReader(resp))
            {
                return (Products)new XmlSerializer(typeof(Products)).Deserialize(reader);
            }
        }

        /// <summary>
        /// Search for products matching the given query string. Search data results has different fields than other API calls.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public Products Search(string query)
        {
            var resp = ApiMethods.CallApiString("product.search.cp", new Dictionary<string, string> { { "query", query } });

            using (StringReader reader = new StringReader(resp))
            {
                return (Products)new XmlSerializer(typeof(Products)).Deserialize(reader);
            }
        }

        /// <summary>
        /// Fetch product search data for the given product id.
        /// Search data results has different fields than other API calls. Results are limited to 256 results.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns></returns>
        public Products SearchForProduct(string productId)
        {
            var resp = ApiMethods.CallApiString("product.searchForProduct.cp", new Dictionary<string, string> { { "productId", productId } });

            using (StringReader reader = new StringReader(resp))
            {
                return (Products)new XmlSerializer(typeof(Products)).Deserialize(reader);
            }
        }

        /// <summary>
        /// Fetch product search data for the given store id and merchandise ids. 
        /// Search data results have different fields than other API calls.
        /// </summary>
        /// <param name="storeId">The store identifier.</param>
        /// <param name="merchandiseIds">
        ///     Ids of merchandise. Use a comma-separated list of ids or one 'merchandiseIds' parameter per id.
        ///     Exclude merchandise by listing negative ids.
        ///     If any negative ids are found the inverse set of those negatives will be returned (positive ids will be ignored if negatives occur.)
        /// </param>
        /// <returns></returns>
        public Products SearchForProductsOfStore(string storeId, string merchandiseIds)
        {
            var resp = ApiMethods.CallApiString("product.searchForProductsOfStore.cp",
                new Dictionary<string, string>
                {
                    {"storeId", storeId},
                    {"merchandiseIds", merchandiseIds}
                });

            using (StringReader reader = new StringReader(resp))
            {
                return (Products)new XmlSerializer(typeof(Products)).Deserialize(reader);
            }
        }

        /// <summary>
        /// Fetch product search data for the given store id and merchandise ids. 
        /// Search data results have different fields than other API calls.
        /// </summary>
        /// <param name="storeId">The store identifier.</param>
        /// <param name="merchandiseIds">
        ///     Ids of merchandise. Use a comma-separated list of ids or one 'merchandiseIds' parameter per id.
        ///     Exclude merchandise by listing negative ids.
        ///     If any negative ids are found the inverse set of those negatives will be returned (positive ids will be ignored if negatives occur.)
        /// </param>
        /// <param name="page">The page number.</param>
        /// <param name="pageSize">The page size (results per page).</param>
        /// <returns></returns>
        public Products SearchForProductsOfStore(string storeId, string merchandiseIds, int page, int pageSize)
        {
            var resp = ApiMethods.CallApiString("product.searchForProductsOfStore.cp",
                new Dictionary<string, string>
                {
                    {"storeId", storeId},
                    {"merchandiseIds", merchandiseIds},
                    {"page", page.ToString()},
                    {"pageSize", pageSize.ToString()}
                });

            using (StringReader reader = new StringReader(resp))
            {
                return (Products)new XmlSerializer(typeof(Products)).Deserialize(reader);
            }
        }

        #endregion

        /// <summary>
        /// Creates a new product definition based on the given merchandise id. 
        /// This product definition may be saved as a new product after adding desired designs.
        /// </summary>
        /// <param name="merchandiseId">The numeric id of a particular merchandise.</param>
        public void Create(int merchandiseId)
        {
            var resp = ApiMethods.CallApiString("product.create.cp", new Dictionary<string, string> { { "merchandiseId", merchandiseId.ToString() } });
        }
    }
}