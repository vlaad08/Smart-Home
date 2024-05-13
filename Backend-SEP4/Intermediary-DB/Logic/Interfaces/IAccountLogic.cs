﻿using DBComm.Shared;

public interface IAccountLogic
{

    Task<Member> GetAdmin(string login, string password);
    Task RegisterMember(string username, string password);
    Task<Member> GetMember(String login, string password);
    Task Delete(string username);
    Task EditUsername(string oldUsername, string newUsername);
    Task EditPassword(string username, string oldPassword, string newPassword);
    Task ToggleAdmin();

    Task<Member> GetAdmin(string login, string password);
    Task<Member> RegisterMember(string username, string password);
    Task<Member> GetMember(string login, string password);
    Task RegisterAdmin(string username, string password);

}
