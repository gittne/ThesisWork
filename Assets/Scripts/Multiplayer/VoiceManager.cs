using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Vivox;
using VivoxUnity;
using System;

public class VoiceManager : MonoBehaviour
{
    public ILoginSession LoginSession;

    private void Start()
    {
        //Login("hey");
        //JoinChannel("1", )
    }

    private void OnApplicationQuit()
    {
        if(LoginSession != null) LoginSession.Logout();
    }

    public void JoinChannel(string channelName, ChannelType channelType, bool connectAudio, bool connectText, bool transmissionSwitch = true, Channel3DProperties properties = null)
    {
        if (LoginSession.State == LoginState.LoggedIn)
        {
            Channel channel = new Channel(channelName, channelType, properties);

            IChannelSession channelSession = LoginSession.GetChannelSession(channel);

            channelSession.BeginConnect(connectAudio, connectText, transmissionSwitch, channelSession.GetConnectToken(), ar =>
            {
                try
                {
                    channelSession.EndConnect(ar);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Could not connect to channel: {e.Message}");
                    return;
                }
            });
        }
        else
        {
            Debug.LogError("Can't join a channel when not logged in.");
        }
    }

    public void Login(string displayName = null)
    {
        var account = new Account(displayName);
        bool connectAudio = true;
        bool connectText = true;

        LoginSession = VivoxService.Instance.Client.GetLoginSession(account);
        LoginSession.PropertyChanged += LoginSession_PropertyChanged;

        LoginSession.BeginLogin(LoginSession.GetLoginToken(), SubscriptionMode.Accept, null, null, null, ar =>
        {
            try
            {
                LoginSession.EndLogin(ar);
            }
            catch (Exception e)
            {
                Debug.LogError($"Could not connect to channel: {e.Message}");
                // Unbind any login session-related events you might be subscribed to.
                // Handle error
                return;
            }

            // At this point, we have successfully requested to login. 
            // When you are able to join channels, LoginSession.State will be set to LoginState.LoggedIn.
            // Reference LoginSession_PropertyChanged() 
        });
    }

    private void LoginSession_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        var loginSession = (ILoginSession)sender;
        if (e.PropertyName == "State")
        {
            if (loginSession.State == LoginState.LoggedIn)
            {
                bool connectAudio = true;
                bool connectText = true;

                // This puts you into an echo channel where you can hear yourself speaking.
                // If you can hear yourself, then everything is working and you are ready to integrate Vivox into your project.
                JoinChannel("TestChannel", ChannelType.Echo, connectAudio, connectText);
                // To test with multiple users, try joining a non-positional channel.
                // JoinChannel("MultipleUserTestChannel", ChannelType.NonPositional, connectAudio, connectText);
            }
        }
    }
}
