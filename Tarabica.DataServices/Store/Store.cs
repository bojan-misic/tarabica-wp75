using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;

namespace Tarabica.DataServices.Store
{
    public abstract class Store
    {
        protected byte[] GetBytes(string storeName)
        {
            lock (this)
            {
                using (var filesystem = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (!filesystem.FileExists(storeName))
                    {
                        return null;
                    }
                    else
                    {
                        using (var rdr = new BinaryReader(
                            new IsolatedStorageFileStream(storeName, FileMode.Open, filesystem)))
                        {
                            using (var ms = new MemoryStream())
                            {
                                var buffer = new byte[4096];
                                int bytesRead = 0;
                                while ((bytesRead = rdr.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    ms.Write(buffer, 0, bytesRead);
                                }
                                return ms.ToArray();
                            }
                        }
                    }
                }
            }
        }

        protected T GetDataFromJson<T>(string storeName)
            where T : new()
        {
            lock (this)
            {
                using (var filesystem = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (!filesystem.FileExists(storeName))
                    {
                        return new T();
                    }
                    else
                    {
                        using (var rdr = new StreamReader(
                            new IsolatedStorageFileStream(storeName, FileMode.Open, filesystem)))
                        {

                            return JsonConvert.DeserializeObject<T>(rdr.ReadToEnd());
                        }
                    }
                }
            }
        }

        protected void DeleteStore(string storeName)
        {
            lock (this)
            {
                using (var filesystem = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    filesystem.DeleteFile(storeName);
                }
            }
        }

        protected void StoreBytes(string storeName, byte[] data)
        {
            lock (this)
            {
                using (var filesystem = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (var sw = new BinaryWriter(
                        new IsolatedStorageFileStream(storeName, FileMode.Create, filesystem)))
                    {
                        sw.Write(data);
                    }
                }
            }
        }

        protected void StoreDataToJson(string storeName, object obj)
        {
            lock (this)
            {
                using (var filesystem = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (var sw = new StreamWriter(
                        new IsolatedStorageFileStream(storeName, FileMode.Create, filesystem)))
                    {
                        sw.Write(JsonConvert.SerializeObject(obj));
                    }
                }
            }
        }
    }
}
