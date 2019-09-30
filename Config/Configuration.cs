﻿using Nett;
using Nett.Coma;

namespace GoryMoon.StreamEngineer.Config
{

    public class Configuration
    {
        public static Config<TokenConfig> Token;
        public static Config<PluginConfig> Plugin;

        
        public class TokenConfig
        {
            

            [TomlComment("Login to Streamlabs and go to this link: https://streamlabs.com/dashboard#/settings/api-settings")]
            [TomlComment("Click on 'API Tokens' and copy the 'Your Socket API Token' here")]
            public string StreamlabsToken { get; set; } = "";

            public static void Init(string path)
            {
                Token = Nett.Coma.Config.CreateAs()
                    .MappedToType(() => new TokenConfig())
                    .StoredAs(builder =>
                        builder.CustomStore(CustomFileStore.Create($"{path}/token-settings{Toml.FileExtension}")))
                    .Initialize();
            }
        }

        public class PluginConfig
        {
            [TomlComment("Shows a popup when the game start to inform about this file, auto sets to false")]
            public bool ShowMenuPopup { get; set; } = true;

            [TomlComment("If donations not matching an action should perform the closest action with lower donation goal")]
            public bool FuzzyDonations { get; set; } = false;

            [TomlComment("If subscriptions not matching an action should perform the closest action with lower sub goal")]
            public bool FuzzySubs { get; set; } = false;

            [TomlComment("If bits not matching an action should perform the closest action with lower bits goal")]
            public bool FuzzyBits { get; set; } = false;

            [TomlComment("Display name of your steam account so actions know what player to target")]
            public string SteamName { get; set; } = "";

            [TomlComment("Here you can setup what actions are triggered by what event, each action can have multiple events, separate with a ','")]
            [TomlComment("For the following types you can replace '?' with a value of your choice")]
            [TomlComment("Donation: Don-?\nTwitch Subscription: TSub-?\nTwitch Follow: TFollow\nTwitch Bits: TBits-?\nTwitch Host(value is lowest viewer count): THost-?")]
            [TomlComment("Twitch Raid(value is lowest viewer count): TRaid-?\nYouTube Subscription: YSub\nYouTube Sponsor: YSponsor-?\nYouTube Superchat: YSuper-?")]
            [TomlComment("Mixer Subscription: MSub-?\nMixer Follow: MFollow\nMixer Host(value is lowest viewer count): MHost-?")]
            public ActionSettings Actions { get; set; } = new ActionSettings();

            public EventMessages Events { get; set; } = new EventMessages();

            public static void Init(string path)
            {
                Plugin = Nett.Coma.Config.CreateAs()
                    .MappedToType(() => new PluginConfig())
                    .StoredAs(builder =>
                        builder.CustomStore(CustomFileStore.Create($"{path}/settings{Toml.FileExtension}")))
                    .Initialize();
            }

            public class ActionSettings
            {
                public Action MeteorShower { get; set; } = new Action(new[] {"Don-20"}, "Let it RAIN!");

            }

            public class Action
            {
                public Action()
                    : this(null)
                {
                }

                public Action(string[] events = null, string message = "")
                {
                    Events = events ?? new string[0];
                    Message = message;
                }

                public string[] Events { get; set; }

                public string Message { get; set; }
            }



            public class EventMessages
            {

                [TomlComment("0:Name of donor, 1:Formatted amount donated")]
                public SingleMessageEvent Donation { get; set; } = new SingleMessageEvent("{0} donated {1}!", true);

                public TwitchSubscriptionEvent TwitchSubscription { get; set; } = new TwitchSubscriptionEvent();

                [TomlComment("0:Name of cheerer, 1:Amount cheered")]
                public SingleMessageEvent TwitchBits { get; set; } = new SingleMessageEvent("{0} cheer with {1} bits!", true);

                [TomlComment("0:Name of follower")]
                public SingleMessageEvent TwitchFollowed { get; set; } = new SingleMessageEvent("{0} followed!");

                [TomlComment("0:Name of user hosting, 1:Amount of viewers")]
                public SingleMessageEvent TwitchHost { get; set; } = new SingleMessageEvent("{0} hosted with {1} viewers!");

                [TomlComment("0:Name of raider, 1:Amount of viewers")]
                public SingleMessageEvent TwitchRaid { get; set; } = new SingleMessageEvent("{0} raided with {1} viewers!");

                [TomlComment("0:Name of subscriber")]
                public SingleMessageEvent YoutubeSubscription { get; set; } = new SingleMessageEvent("{0} subscribed!");

                public YoutubeSponsorEvent YoutubeSponsor { get; set; } = new YoutubeSponsorEvent();

                [TomlComment("0:Name of superchatter, 1:Formatted amount superchatted")]
                public SingleMessageEvent YoutubeSuperchat { get; set; } = new SingleMessageEvent("{0} superchatted with {1}!");

                public MixerSponsorEvent MixerSubscription { get; set; } = new MixerSponsorEvent();

                [TomlComment("0:Name of follower")]
                public SingleMessageEvent MixerFollowMessage { get; set; } = new SingleMessageEvent("{0} followed!");

                [TomlComment("0:Name of user hosting, 1:Amount of viewers")]
                public SingleMessageEvent MixerHostMessage { get; set; } = new SingleMessageEvent("{0} hosted with {1} viewers!");
            }

            public class SingleMessageEvent
            {
                public SingleMessageEvent() {}

                public SingleMessageEvent(string message, bool alwaysSendMessage = false)
                {
                    Message = message;
                    AlwaysSendMessage = alwaysSendMessage;
                }

                public string Message { get; set; }

                [TomlComment("If an message should be sent even if no action is executed")]
                public bool AlwaysSendMessage { get; set; }
            }

            public class TwitchSubscriptionEvent
            {
                [TomlComment("0:Name of subscriber, 1:Tier of subscription")]
                public string NewMessage { get; set; } = "{0} subscribed with a {1} subscription!";

                [TomlComment("0:Name of subscriber, 1:Tier of subscription, 2:Amount of months")]
                public string ResubMessage { get; set; } = "{0} subscribed with a {1} subscription for {2} months!";

                [TomlComment("If an message should be sent event if no action is executed")]
                public bool AlwaysSendMessage { get; set; } = true;

                public string Tier1 { get; set; } = "Tier 1";
                public string Tier2 { get; set; } = "Tier 2";
                public string Tier3 { get; set; } = "Tier 3";
            }

            public class YoutubeSponsorEvent
            {
                [TomlComment("0:Name of sponsor")]
                public string NewMessage { get; set; } = "{0} sponsored the channel!";

                [TomlComment("0:Name of sponsor, 1:Amount of months")]
                public string ResubMessage { get; set; } = "{0} sponsored the channel for {1} months!";

                [TomlComment("If an message should be sent event if no action is executed")]
                public bool AlwaysSendMessage { get; set; } = false;
            }

            public class MixerSponsorEvent
            {
                [TomlComment("0:Name of subscriber")]
                public string NewMessage { get; set; } = "{0} subscribed to the channel!";

                [TomlComment("0:Name of subscriber, 1:Amount of months")]
                public string ResubMessage { get; set; } = "{0} subscribed for {1} months!";

                [TomlComment("If an message should be sent event if no action is executed")]
                public bool AlwaysSendMessage { get; set; } = false;
            }
        }
    }
}