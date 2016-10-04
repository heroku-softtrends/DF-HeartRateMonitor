# Dreamforce'16 
##IOT Cloud & Heroku Enterprise: *Fitness Monitoring Ingestion App*

**How to Deploy & Configure the Demo**

* Ensure you have a [Heroku Account](https://signup.heroku.com) and are signed into it
* Click the Deploy to Heroku Button below
* Leave the pre-populated Configuration Vars as-is, unless asked to modify them
* Confirm the Deploy
* Watch the magic of IOT Cloud & Heroku together!

**What the Configuration Variables mean**

- `deviceID` - identifies the Heroku App to IOT Cloud as a "unique Heartrate Monitoring Device" and must be unique
- `deviceToken` - allows IOT Cloud to authorize incoming messages to its Endpoint
- `endpointURL` - URL the Heroku App uses to send "Heartrate Monitoring data" (aka the IOT Endpoint)
- `IRON_MQ_PROJECT_ID` - allows the Heroku MQ listener to know where incoming messages should be stored (where its queues live)
- `IRON_MQ_TOKEN` - allows the Heroku MQ to authorize incoming messages from IOT Cloud to its Queue Endpoint
- `queueURL` - URL the IOT Cloud Orchestration uses to send "HeartRate Monitoring data" (aka the Heroku App Endpoint)

**Technologies Used in this Demo**

1. Heroku Enterprise: *used to generate large amounts of real-world data & send it to the IOT Cloud endpoint*
2. IOT Cloud: *handles message ingestion, orchestration & business workflow for the 'Heroku IOT Demo App'*
3. Heroku MQ (Message Queue) Add-On: *used to handle UX change responses from IOT Cloud to the Heroku App*
4. ASP.NET Core: *language the Heroku App was written in (refer to [.NET Buildpack Support](http://www.dotnetbuildpacks.com))*

[![Deploy](https://www.herokucdn.com/deploy/button.svg)](https://heroku.com/deploy?template=https://github.com/heroku-softtrends/DF-HeartRateMonitor)

**What to Expect Once Deployed**

![alt text](https://s3.amazonaws.com/herokumximages/heroku-heartmonitor.png "Heroku IOT Cloud Heart Monitor App")

1. Click *"Start Sending Data"* and wait; After detecting inactivity, the app will alert you to *"Increase your BPMs"*
2. Drag the *"Heartrate Goal slider"*, indicating the BPM level you'd like to meet during your fitness routine; the app will alert you with a message stating *"Perfect Heartrate Range"* when you're in an ideal range of 125-174 BPMs
3. Drag the *"Heartrate Goal Slider"* to the far right, indicating you want to perform strenuous exercise; the app will alert you with a message stating *"Max Heartrate...Take a Break"* indicating you should reduce your BPMs

**Support or Questions**

* Contact [us](mailto:david@heroku.com) if you have any questions about this Dreamforce'16 IOT Cloud & Heroku Demo
