using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using FilmBarcodes.Common.Models.CafePress;
using FilmBarcodes.Common.Models.Settings;

namespace CafePress.Api
{
    public class MerchandiseMethods
    {
        private SettingsWrapper SettingsWrapper { get; set; }
        private ApiMethods ApiMethods { get; set; }

        public MerchandiseMethods(SettingsWrapper settingsWrapper)
        {
            SettingsWrapper = settingsWrapper;

            ApiMethods = new ApiMethods(SettingsWrapper);
        }

        #region find

        /// <summary>
        /// Find merchandise by its Id and Sales Channel Id, returning the full merchandise definition.
        /// </summary>
        /// <param name="id">The numeric id of the merchandise.</param>
        /// <returns></returns>
        public Merchandise Find(int id)
        {
            var resp = ApiMethods.CallApiString("merchandise.find.cp", new Dictionary<string, string> { { "id", id.ToString() } });

            using (StringReader reader = new StringReader(resp))
            {
                return (Merchandise)new XmlSerializer(typeof(Merchandise)).Deserialize(reader);
            }
        }

        /// <summary>
        /// Find merchandise by its Id and Sales Channel Id, returning the full merchandise definition.
        /// </summary>
        /// <param name="id">The numeric id of the merchandise.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public Merchandise Find(int id, string userId)
        {
            var resp = ApiMethods.CallApiString("merchandise.find.cp", new Dictionary<string, string> { { "id", id.ToString() }, {"userId", userId} });

            using (StringReader reader = new StringReader(resp))
            {
                return (Merchandise)new XmlSerializer(typeof(Merchandise)).Deserialize(reader);
            }
        }

        /// <summary>
        /// Find merchandise by its name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public Merchandise FindByName(string name)
        {
            var resp = ApiMethods.CallApiString("merchandise.findByName.cp", new Dictionary<string, string> { { "name", name } });

            using (StringReader reader = new StringReader(resp))
            {
                return (Merchandise)new XmlSerializer(typeof(Merchandise)).Deserialize(reader);
            }
        }

        #endregion

        #region list and get

        /// <summary>
        /// Returns the current pricing for each sales channel.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="currency">The currency.</param>
        public void GetPricing(int id, string currency)
        {
            var resp = ApiMethods.CallApiString("merchandise.getPricing.cp", new Dictionary<string, string> { { "id", id.ToString() }, { "currency", currency } });
        }
        
        /// <summary>
        /// Returns a list of full merchandise definitions for all merchandise.
        /// </summary>
        /// <returns></returns>
        public Merchandises List()
        {
            var resp = ApiMethods.CallApiString("merchandise.list.cp");

            using (StringReader reader = new StringReader(resp))
            {
                return (Merchandises)new XmlSerializer(typeof(Merchandises)).Deserialize(reader);
            }
        }

        /// <summary>
        /// Returns a list of category's merchandise ids
        /// </summary>
        /// <returns></returns>
        public Values ListBySearchCategoryNo(string categoryNo)
        {
            var resp = ApiMethods.CallApiString("merchandise.listBySearchCategoryNo.cp", new Dictionary<string, string>{{"categoryNo", categoryNo}});

            using (StringReader reader = new StringReader(resp))
            {
                return (Values)new XmlSerializer(typeof(Values)).Deserialize(reader);
            }
        }

        /// <summary>
        /// Returns a list of all merchandise ids.
        /// </summary>
        /// <returns></returns>
        public Values ListIds()
        {
            var resp = ApiMethods.CallApiString("merchandise.listIds.cp");

            using (StringReader reader = new StringReader(resp))
            {
                return (Values)new XmlSerializer(typeof(Values)).Deserialize(reader);
            }
        }

        #endregion
    }
}