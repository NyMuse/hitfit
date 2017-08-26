using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Drive.v3;
using Google.Apis.Util.Store;
using hitfit.google.auth.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace hitfit.app.Google
{
    public class AppFlowMetadata : FlowMetadata
    {
        private static string commonUser = "hitfit.com.ua@gmail.com";
        private static string refreshToken = string.Empty;

        private static readonly IAuthorizationCodeFlow flow =
            new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = "703137161297-kcdmjcjk5caa9ampgvraqa4ktt5nsqit.apps.googleusercontent.com",
                    ClientSecret = "2XXbusAQkVUsY7MvoxXQRlIi"
                },
                Scopes = new[] { DriveService.Scope.Drive },
                //DataStore = new FileDataStore("Drive.Api.Auth.Store")
                DataStore = new GDriveMemoryDataStore() // FileDataStore("Drive.Api.Auth.Store")
            });

        public override string GetUserId(Controller controller)
        {
            // In this sample we use the session to store the user identifiers.
            // That's not the best practice, because you should have a logic to identify
            // a user. You might want to use "OpenID Connect".
            // You can read more about the protocol in the following link:
            // https://developers.google.com/accounts/docs/OAuth2Login.
            //var user = controller.Session["user"];
            //if (user == null)
            //{
            //    user = Guid.NewGuid();
            //    controller.Session["user"] = user;
            //}
            //return user.ToString();
            return commonUser;
        }

        public override IAuthorizationCodeFlow Flow
        {
            get { return flow; }
        }
    }

    internal class GDriveMemoryDataStore : IDataStore
    {
        private Dictionary<string, TokenResponse> _store;
        private Dictionary<string, string> _stringStore;

        //private key password: notasecret

        public GDriveMemoryDataStore()
        {
            _store = new Dictionary<string, TokenResponse>();
            _stringStore = new Dictionary<string, string>();
        }

        public GDriveMemoryDataStore(string key, string refreshToken)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");
            if (string.IsNullOrEmpty(refreshToken))
                throw new ArgumentNullException("refreshToken");

            _store = new Dictionary<string, TokenResponse>();

            // add new entry
            StoreAsync<TokenResponse>(key,
                new TokenResponse() { RefreshToken = refreshToken, TokenType = "Bearer" }).Wait();
        }

        /// <summary>
        /// Remove all items
        /// </summary>
        /// <returns></returns>
        public async Task ClearAsync()
        {
            await Task.Run(() =>
            {
                _store.Clear();
                _stringStore.Clear();
            });
        }

        /// <summary>
        /// Remove single entry
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task DeleteAsync<T>(string key)
        {
            await Task.Run(() =>
            {
                // check type
                AssertCorrectType<T>();

                if (typeof(T) == typeof(string))
                {
                    if (_stringStore.ContainsKey(key))
                        _stringStore.Remove(key);
                }
                else if (_store.ContainsKey(key))
                {
                    _store.Remove(key);
                }
            });
        }

        /// <summary>
        /// Obtain object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string key)
        {
            // check type
            AssertCorrectType<T>();

            if (typeof(T) == typeof(string))
            {
                if (_stringStore.ContainsKey(key))
                    return await Task.Run(() => { return (T)(object)_stringStore[key]; });
            }
            else if (_store.ContainsKey(key))
            {
                return await Task.Run(() => { return (T)(object)_store[key]; });
            }
            // key not found
            return default(T);
        }

        /// <summary>
        /// Add/update value for key/value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task StoreAsync<T>(string key, T value)
        {
            return Task.Run(() =>
            {
                if (typeof(T) == typeof(string))
                {
                    if (_stringStore.ContainsKey(key))
                        _stringStore[key] = (string)(object)value;
                    else
                        _stringStore.Add(key, (string)(object)value);
                }
                else
                {
                    if (_store.ContainsKey(key))
                        _store[key] = (TokenResponse)(object)value;
                    else
                        _store.Add(key, (TokenResponse)(object)value);
                }
            });
        }

        /// <summary>
        /// Validate we can store this type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private void AssertCorrectType<T>()
        {
            if (typeof(T) != typeof(TokenResponse) && typeof(T) != typeof(string))
                throw new NotImplementedException(typeof(T).ToString());
        }
    }
}
