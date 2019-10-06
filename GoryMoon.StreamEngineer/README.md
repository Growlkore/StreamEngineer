﻿# StreamEngineer

To get started using this plugin you need to add the path to the launch options of Space Engineers.

Extract this folder anywhere and get it's path, save this for later.

 1. Go to `Space Engineers` in steam. 
 2. Open the properties of it.
 3. Click `SET LAUNCH OPTIONS...`
 4. Add `-plugin <path>/StreamEngineer.dll` where `<path>` is the one from above 
 5. Open the `settings.toml` and enter the `SteamName` 
 6. Open the `token-settings.toml` and enter the `StreamLabsToken`
 
## Configuring events

To edit the events open the `events.json`, can be done while game is running.

### Actions
|      Type        | Comments                                                                                       |               Additional parameters              |
|:----------------:|------------------------------------------------------------------------------------------------|:------------------------------------------------:|
|  `"meteors"`     | Spawns `amount` of meteor storms, `radius` of `1.0` is like vanilla, lower is closer to player | `"radius": 1.0, "amount": "number" or "event"`   |
| `"power_off"`    | Turns off the power of the current vehicle                                                     |                                                  |
| `"power_toggle"` | Toggles the the power of the current vehicle                                                   |                                                  |
|   `"refill"`     | Refills health, O2, H2 and energy                                                              |                                                  |

`$event$` can be used on supported parameters to use the amount data from the event. 
For example the amount of months a user have subscribed.

### Conditions

You can put conditions on events to only run when those are met, you can have multiple conditions and only one of them needs to be met.

All parameters in a condition is optional except the `type` parameter. 
Not setting one of `to` or `from` will match up to or from to everything depending on which one.

You can leave the condition array empty to always run the action.

```json
{
  "type": "twitch_bits",
  "from": 100,
  "to": 500
}
```
> Example condition

The available types are:

|          Type          |           `$event$`           |
|:----------------------:|:-----------------------------:|
| `donation`             | Donation amount               |
| `twitch_subscription`  | Months subscribed             |
| `twitch_bits`          | The amount of bits            |
| `twitch_follow`        |                               |
| `twitch_host`          | The amount of viewers hosted  |
| `twitch_raid`          | The amount of viewers raiding |
| `youtube_subscription` |                               |
| `youtube_sponsor`      | Months sponsored              |
| `youtube_superchat`    | The amount superchatted       |
| `mixer_follow`         |                               |
| `mixer_subscription`   | Months subscribed             |
| `mixer_host`           | The amount of viewers hosted  |

### Example

You can have multiple of the same event with different conditions

```json
[
  {
    "type": "meteors",
    "action": {
      "conditions": [
        {
          "type": "twitch_bits",
          "from": 100,
          "to": 500
        }
      ],
      "message": "Let it RAIN!",
      "radius": 1.0,
      "amount": "1"
    }
  },
  {
    "type": "meteors",
    "action": {
      "conditions": [
        {
          "type": "twitch_subscription",
          "from": 6
        }
      ],
      "message": "Let it RAIN!",
      "radius": 0.2,
      "amount": "event / 5"
    }
  }
]
```