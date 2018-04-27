﻿using OsuDataDistributeRestful.Server;
using OsuDataDistributeRestful.Server.Api;
using OsuLiveStatusPanel;
using Sync.Plugins;
using System.IO;
using System.Linq;

namespace OsuDataDistributeRestful.Api
{
    [Route("/api/olsp")]
    internal class OlspApis : IApi
    {
        private OsuLiveStatusPanelPlugin olsp;

        public OlspApis(Plugin olsp_plguin)
        {
            OsuLiveStatusPanelPlugin olsp = olsp_plguin as OsuLiveStatusPanelPlugin;
        }

        [Route("/{providable_data_name}")]
        public object GetDictValue(string providable_data_name)
        {
            if (!olsp.EnumProvidableDataName().Any(p => p == providable_data_name))
                return null;

            var result = olsp.GetData(providable_data_name);
            return new
            {
                status = result != null,
                value = result
            };
        }

        [Route("/backgroundImage")]
        public StreamResult GetBackgoundImage()
        {
            var result = olsp.GetData("olsp_bg_path") as string;
            if (string.IsNullOrEmpty(result)) return new StreamResult(null);

            string ext = Path.GetExtension(result);
            var fs = File.Open(result, FileMode.Open, FileAccess.Read, FileShare.Read);

            return new StreamResult(fs)
            {
                ContentType = GetContentType(ext)
            };
        }

        [Route("/outputBackgroundImage")]
        public StreamResult GetOuputBackgoundImage()
        {
            var result = olsp.GetData("olsp_bg_save_path") as string;
            if (string.IsNullOrEmpty(result)) return new StreamResult(null);

            string ext = Path.GetExtension(result);
            var fs = File.Open(result, FileMode.Open, FileAccess.Read, FileShare.Read);

            return new StreamResult(fs)
            {
                ContentType = GetContentType(ext)
            };
        }

        [Route("/modsImage")]
        public StreamResult GetModsImage()
        {
            var result = olsp.GetData("olsp_mod_save_path") as string;
            if (string.IsNullOrEmpty(result)) return new StreamResult(null);

            string ext = Path.GetExtension(result);
            var fs = File.Open(result, FileMode.Open, FileAccess.Read, FileShare.Read);

            return new StreamResult(fs)
            {
                ContentType = GetContentType(ext)
            };
        }

        private string GetContentType(string fileExtention)
        {
            switch (fileExtention)
            {
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";

                case ".png":
                    return "image/png";

                default:
                    return "application/octet-stream";
            }
        }
    }
}