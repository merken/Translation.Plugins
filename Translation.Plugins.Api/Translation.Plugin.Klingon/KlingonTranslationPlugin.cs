using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Translation.Plugin.Contract;

namespace Translation.Plugin.Klingon
{
    public class KlingonTranslationPlugin : ITranslationPlugin
    {
        public async Task<TranslationResponse> Translate(TranslationRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
