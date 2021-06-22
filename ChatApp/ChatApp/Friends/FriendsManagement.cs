using ChatApp.Models;
using Plugin.ContactService.Shared;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;


namespace ChatApp.Friends
{
    public class FriendsManagement
    {
       
        
        public async System.Threading.Tasks.Task<ObservableCollection<Friend>> returnListOfFriendsAsync()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync<ContactsPermission>();
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Contacts))
                    {
                        
                    }
                    status = await CrossPermissions.Current.RequestPermissionAsync<ContactsPermission>();
                }

                if (status == PermissionStatus.Granted)
                {
                    
                    var contacts = await Plugin.ContactService.CrossContactService.Current.GetContactListAsync();
                    var friendList = new ObservableCollection<Friend>();
                    foreach(Contact contact in contacts)
                    {
                        string formattedPhoneNumber = convertPhoneNumberToCorrectFormat(contact.Number);
                        Friend friend = new Friend(formattedPhoneNumber, contact.Name);
                        friendList.Add(friend);
                    }
                    return friendList;
                }
                else if (status != PermissionStatus.Unknown)
                {
                    
                    Console.WriteLine("CONTACT PERMISSION NOT GRANTED. CAN'T DO ANYTHING");
                }
            }
            catch (Exception ex)
            {
                
            }

            return null;
        }

        private string convertPhoneNumberToCorrectFormat(string unformattedPhoneNum)
        {
            string formattedPhoneNumber = string.Concat(unformattedPhoneNum.Where(char.IsDigit)); 
            int numOfDigits = formattedPhoneNumber.Length;
            if(numOfDigits > 9)
            {
                formattedPhoneNumber = formattedPhoneNumber.Substring(1, numOfDigits - 1); 
            }
            return formattedPhoneNumber;
        }
    }
}
