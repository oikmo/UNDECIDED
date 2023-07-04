using Discord;
using UnityEngine;
using System;
using System.Diagnostics;

public class DiscordController : MonoBehaviour
{
    public string Details;
    public string State;
    public string Image;
    public Discord.Discord discord;
	
	void Start()
    {
		
		if(!Application.isEditor) 
		{
			if (Process.GetProcessesByName("discord") != null)
			{
				discord = new Discord.Discord(972224735180103810, (System.Int32)CreateFlags.Default);
				var audioManager = discord.GetVoiceManager();
				var voice = new Discord.Presence {
                    


                };

				var activityManager = discord.GetActivityManager();
				var activity = new Discord.Activity
				{
					Details = Details,
					State = State,

					/*Timestamps =
					{
					Start = 5,
				},*/
					Assets =
				{
				LargeImage = Image, // Larger Image Asset Value
				//LargeText = "foo largeImageText", // Large Image Tooltip
				//SmallImage = "foo smallImageKey", // Small Image Asset Value
				//SmallText = "foo smallImageText", // Small Image Tooltip
				},
					/*Party =
					{
					Id = "foo partyID",
					Size = {
					  CurrentSize = 1,
					  MaxSize = 4,
				   },
				},*/
					/*Secrets =
					{
					  Match = "foo matchSecret",
					  Join = "foo joinSecret",
					  Spectate = "foo spectateSecret",
					},*/
					Instance = true,
				};
				activityManager.UpdateActivity(activity, (result) =>
				{
					if (result == Discord.Result.Ok)
					{
						print("Success!");
					}
					else
					{
						print("Failed");
					}
				});
			}
			
			
		}
    }

    void Update()
    {
		if(!Application.isEditor && Process.GetProcessesByName("discord") != null) {
			discord.RunCallbacks();
		}
        
    }
    void onApplicationQuit()
    {
		if(!Application.isEditor) {
			var activityManager = discord.GetActivityManager();
		}
    }
}