using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushNotification : MonoBehaviour
{
    public void Start() {
        Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
        Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
    }

    public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token) {
        UnityEngine.Debug.Log("Received Registration Token: " + token.Token);
    }

    public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e) {
        UnityEngine.Debug.Log("Received a new message from: " + e.Message.From);
        
        ShowInAppMessage(e.Message.Notification.Title, e.Message.Notification.Body);
    }
    
    private void ShowInAppMessage(string title, string body)
    {
        // Aquí puedes implementar la lógica para mostrar el mensaje en tu interfaz de usuario Unity
        Debug.Log("In-app Message: " + title + " - " + body);
    }
}
