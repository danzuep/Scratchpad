## Trade Processing Architecture with Event Sourcing and Real-Time Notifications

> This diagram illustrates a robust trade processing system architecture designed for reliability, consistency, and real-time updates. The architecture leverages event sourcing, asynchronous messaging, and idempotent API handling to ensure reliable, consistent trade processing with real-time client feedback and Command Query Responsibility Segregation.  

```mermaid
flowchart TD
    subgraph Client/UI
        UI["Trade UI<br>(Web/Mobile)"]
    end

    subgraph Gateway
        APIGW["API Gateway"]
    end

    subgraph Core
        TradeSvc["Trade Service<br>(Idempotency Key)"]
        Booking["Booking Workers"]
        EventStore[("Event Store<br>(Append-only)")]
        DB[("Trade View<br>Database")]
        Broker[["Message Broker"]]
        Notify["Notification Service"]
    end

    UI -- Submits Trade --> APIGW
    APIGW -- Forwards Request --> TradeSvc
    TradeSvc -- Write Event --> EventStore
    TradeSvc -- Publish Event --> Broker
    Broker -- Event --> Booking
    Booking -- Update Event --> EventStore
    Booking -- Update Status --> DB
    DB -- Statuses/Views --> TradeSvc
    TradeSvc -- Query for UI --> DB

    Broker -- Notify Event --> Notify
    Notify -- Push Updates --> UI

    %% Roll forward/backward arrows
    EventStore -- Roll Forward/Backward --> TradeSvc

    %% Real-time status update arrows
    Booking -- Publish Completion Event --> Broker

    style UI fill:#f9f,stroke:#333,stroke-width:1px,color:#000000
    style APIGW fill:#bbf,stroke:#333,stroke-width:1px,color:#000000
    style TradeSvc fill:#bfb,stroke:#333,stroke-width:1px,color:#000000
    style EventStore fill:#ffd,stroke:#333,stroke-width:1px,color:#000000
    style DB fill:#ddf,stroke:#333,stroke-width:1px,color:#000000
    style Broker fill:#fbb,stroke:#333,stroke-width:1px,color:#000000
    style Booking fill:#ccc,stroke:#333,stroke-width:1px,color:#000000
    style Notify fill:#fcf,stroke:#333,stroke-width:1px,color:#000000
```

### Client/UI Layer
The user interacts with the system via a Trade UI, accessible through web or mobile devices.  

### Gateway
An API Gateway handles incoming trade requests from the UI, ensuring idempotency by using idempotency keys to prevent duplicate processing.  

### Core Services

The **Trade Service** receives trade requests from the Gateway and writes corresponding events to an append-only Event Store.  

The **Message Broker** publishes these trade events to downstream components.  

The **Booking Engine** subscribes to events from the broker, processes trades by updating their booking status, and records these updates back into the Event Store and a query-optimized trade Database (DB) containing trade views and statuses.  

The **Trade Service** queries this DB to retrieve current trade statuses for UI requests.  

The **Notification Service** listens to events from the broker and pushes real-time updates back to the Trade UI, keeping the client informed of trade status changes.  

The **Event Store** supports rollforward and rollback capabilities, enabling the Trade Service to reconstruct or adjust trade states by replaying events.  

**Real-time** completion events flow from the Booking Engine through the Message Broker to update the Notification Service and UI promptly.  
