# GlasgowAstro.GuideAlert

A .NET Core console application that monitors PHD2 for StarLoss events and sends real-time alerts to Slack.

## Overview

GlasgowAstro.GuideAlert is a monitoring tool designed for astrophotographers using PHD2 guiding software. The application continuously monitors PHD2's event stream via TCP connection and automatically sends notifications to a Slack channel when star loss events are detected, allowing immediate response to guiding issues during imaging sessions.

## Purpose

During long astrophotography imaging sessions, losing the guide star can result in ruined sub-exposures and wasted imaging time. This application provides immediate notification when PHD2 loses the guide star, enabling astrophotographers to:

- Respond quickly to guiding failures
- Minimize lost imaging time
- Monitor remote imaging setups
- Maintain awareness of equipment status during unattended sessions

## How It Works

1. **Initialization**: The application starts and loads configuration from `appsettings.json`
2. **Connection Test**: Establishes TCP connection to PHD2 (default: localhost:4400) and verifies connectivity
3. **Alert Test**: Sends a test message to the configured Slack webhook to verify alert system
4. **Monitoring**: Continuously reads and parses JSON event messages from PHD2's event stream
5. **Detection**: When a "StarLost" event is detected in the stream, triggers an alert
6. **Notification**: Sends configured alert message to Slack webhook for immediate notification

## Technical Stack

- **.NET 8.0**: Cross-platform console application
- **Dependency Injection**: Microsoft.Extensions.DependencyInjection for service management
- **Configuration**: JSON-based configuration with Microsoft.Extensions.Configuration
- **Logging**: Serilog with file and console output
- **HTTP Client**: HttpClientFactory for Slack webhook requests
- **JSON Parsing**: Newtonsoft.Json for event stream deserialization

## Configuration

Configuration is managed through `appsettings.json`:

```json
{
  "GuideAlertSettings": {
    "SlackWebhookUrl": "your-slack-webhook-url",
    "AlertMessage": "Star lost!",
    "TestAlertMessage": "Test alert!",
    "PhdHost": "localhost",
    "PhdPort": 4400,
    "LogPhdEventsToConsole": true
  }
}
```

### Configuration Options

- **SlackWebhookUrl**: Slack incoming webhook URL for alert notifications (required)
- **AlertMessage**: Message sent when star loss is detected
- **TestAlertMessage**: Message sent during initial connection test
- **PhdHost**: PHD2 server hostname (default: localhost)
- **PhdPort**: PHD2 TCP event server port (default: 4400)
- **LogPhdEventsToConsole**: Enable/disable console logging of all PHD2 events

## Prerequisites

- .NET 8.0 or later runtime
- PHD2 guiding software running with event server enabled
- Slack workspace with incoming webhook configured

## Building

```bash
dotnet build
```

## Running

```bash
dotnet run --project GlasgowAstro.GuideAlert
```

## Project Status

Pre-alpha - Under active development

## Links

- Instagram: https://www.instagram.com/glasgowastro
- Website: https://glasgowastro.co.uk

## License

See repository for license information.
