/*
 * Aaron Keith Sanders
 * 
 * G00225605
 * 
 * UA Grantham
 * 
 * IS498: Senior Research Project
 * 
 * Robert Chubbuck
 * 
 * File: AuthEnum.cs
 * 
 * Desc: This file stores the state machines to allow persistence of
 * the logged in/logged out states, as well as the account types of
 * the two different account users: employees and institutions
 * 
 * Date: 25 April 2026
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace SandersCapstoneProject
{
    public enum AuthState
    {
        LoggedOut,
        LoggedIn
    }
    
    public enum AccountType
    {
        None,
        Employee,
        Institution
    }    
}
