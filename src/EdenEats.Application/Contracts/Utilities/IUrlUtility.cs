using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Application.Contracts.Utilities
{
    public interface IUrlUtility
    {
        byte[] EncodeUrlToBase64(string token);
        byte[] DecodeBase64Url(string base64token);
    }
}
