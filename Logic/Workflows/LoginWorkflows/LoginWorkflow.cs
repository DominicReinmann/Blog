﻿using BlogServer.CrossCutting.Logger;
using BlogServer.CrossCutting.Models.Domain;
using BlogServer.Logic.Encryption;
using BlogServer.Logic.Manager.UserManagement;
using System.Text.Json;

namespace BlogServer.Logic.Workflows.LoginWorkflows
{
    public class LoginWorkflow : ILoginWorkflow
    {
        private readonly ILog _log;
        private readonly IUserManager _manager;
        private readonly IEncryptionService _encryptionService;

        public LoginWorkflow(ILog log, IUserManager manager, IEncryptionService encryptionService)
        {
            _log = log;
            _manager = manager;
            _encryptionService = encryptionService;
        }

        public void RunRegisterUser(User user)
        {
            try
            {
                _log.DebugLog(JsonSerializer.Serialize(user));
                user.Password = _encryptionService.Encryption(user.Password);
                _manager.AddUser(user);
            }
            catch (Exception ex)
            {
                _log.ErrorLog($"Error creating user {ex.Message}");
            }
        }

        public User GetUser(string username)
        {
            try
            {
              return  _manager.GetAll().Where(x => x.Username == username).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.ErrorLog($"Error getting user {ex.Message}");
                return new User();
            }
        }

        public bool RunLoginUser(string pw, string username)
        {
            try
            {
                if (_encryptionService.CheckPassword(pw, username) == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _log.ErrorLog($" {ex.Message}");
            }
            return false;
        }

        public void RunResetPassword(User user)
        {

        }
    }
}
