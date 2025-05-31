## Trade Processing Architecture

> With Event Sourcing and Real-Time Notifications

```mermaid
flowchart TD
    subgraph External
        subgraph Client/UI
            ClientUI["Trade UI<br>(Web/Mobile)"]
            AdminUI["Admin UI<br>(Swagger)"]
        end

        subgraph Gateway
            APIGW["API Gateway<br>(Auth/Rate Limit)"]
        end
    end

    subgraph Internal
        subgraph Queues
            Broker[["Message Broker"]]
            DLQ[["Dead Letter Queue"]]
        end

        subgraph Services
            Audit["Audit Service"]
            TradeSvc["Trade Service<br>(Idempotency)"]
            Booking["Booking Worker(s)"]
            Replay["Event Replay Service"]
            Notify["Notification Service"]
        end
        
        subgraph Data
            EventStore[("Event Store<br>(Append-only)")]
            DB[("Trade View DB<br>(Projections)")]
            Notify["Notification Service"]
        end
    end

    ClientUI -- Submits Trade --> APIGW
    AdminUI <-- Request/Response --> APIGW
    APIGW <-- Request/Response --> Audit
    APIGW -- Forwards Request --> TradeSvc
    TradeSvc -- Write Event --> EventStore
    TradeSvc -- Publish Event --> Broker
    Broker -- Consume Event --> Booking
    Booking -- Update Event --> EventStore
    Booking -- Update Status --> DB
    DB -- Status/View --> TradeSvc
    TradeSvc -- Query for UI --> DB

    Broker -- Notify Event --> Notify
    Notify -- Push Updates --> ClientUI

    EventStore -- Roll Forward/Backward --> Replay
    Replay -- Rebuild Projection --> DB
    EventStore -- Audit Stream --> Audit

    Booking -- On Error --> DLQ
    Notify -- On Error --> DLQ
    DLQ -- Retry --> Booking
    DLQ -- Retry --> Notify

    Booking -- Completion Event --> Broker

    style ClientUI fill:#f9f,stroke:#333,stroke-width:1px,color:#000000
    style AdminUI fill:#e7e,stroke:#333,stroke-width:1px,color:#000000
    style APIGW fill:#bbf,stroke:#333,stroke-width:1px,color:#000000
    style TradeSvc fill:#bfb,stroke:#333,stroke-width:1px,color:#000000
    style EventStore fill:#ffd,stroke:#333,stroke-width:1px,color:#000000
    style DB fill:#ddf,stroke:#333,stroke-width:1px,color:#000000
    style Broker fill:#fbb,stroke:#333,stroke-width:1px,color:#000000
    style Booking fill:#ccc,stroke:#333,stroke-width:1px,color:#000000
    style Notify fill:#fcf,stroke:#333,stroke-width:1px,color:#000000
    style Replay fill:#eef,stroke:#333,stroke-width:1px,color:#000000
    style Audit fill:#ffd,stroke:#333,stroke-width:1px,color:#000000
    style DLQ fill:#fee,stroke:#333,stroke-width:1px,color:#000000
    style DLQ fill:#fee,stroke:#333,stroke-width:1px,color:#000000
```
