
# Trade Processing System Architecture

## Overview of Event-Driven Trade Workflow

```mermaid
flowchart TD
    UI3[Trade UI]
    APIGW3[API Gateway]
    APIFunc3[Trade Lambda]
    EventBus3[[Event Bus]]
    BookingFunc3[Booking Lambda]
    StatusFunc3[Status Updater Lambda]
    DB3[(NoSQL DB)]
    NotifyFunc3[Notification Lambda]

    UI3 --> APIGW3
    APIGW3 --> APIFunc3
    APIFunc3 --> EventBus3
    EventBus3 --> BookingFunc3
    BookingFunc3 --> DB3
    BookingFunc3 --> EventBus3
    EventBus3 --> StatusFunc3
    StatusFunc3 --> DB3
    StatusFunc3 --> EventBus3
    EventBus3 --> NotifyFunc3
    NotifyFunc3 --> UI3
    APIFunc3 -. Roll Forward/Backward .-> DB3
```

The user interacts with the Trade UI, which sends requests to the API Gateway. The gateway forwards these requests to the Trade Lambda function, responsible for handling trade operations. Upon processing, the Trade Lambda publishes events to the Event Bus.

The **Booking Lambda** listens to the Event Bus and processes booking-related events, updating the NoSQL Database with booking information. It also emits further events back to the Event Bus.

The **Status Updater Lambda** subscribes to the Event Bus, updating the trade status in the NoSQL Database and emitting status update events as well.

The **Notification Lambda** consumes events from the Event Bus to send notifications back to the Trade UI, ensuring the user is informed of relevant updates.

The **Trade Lambda** function has a testing capability where it can directly roll forward or backward changes in the NoSQL Database to support testing scenarios.