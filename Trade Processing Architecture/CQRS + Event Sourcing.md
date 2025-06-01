## CQRS + Event Sourcing

> Command Query Responsibility Segregation with Event Sourcing and Real-Time Notifications

```mermaid
flowchart TD
    subgraph API
        ClientUI["UI"]
        APIGW["API Gateway"]
    end

    subgraph Write
        CmdSvc["Command Service"]
        EventStore[("Event Store")]
        EventBus[["Event Bus (Broker)"]]
    end

    subgraph Read
        ProjectionSvc["Projection Service"]
        ReadDB[("Read DB (Projection)")]
    end

    ClientUI -- Request --> APIGW
    APIGW -- Command --> CmdSvc
    CmdSvc -- Append Event --> EventStore
    EventStore -- Publish Event --> EventBus
    EventBus -- Notify --> ProjectionSvc
    ProjectionSvc -- Update --> ReadDB
    ReadDB -- Query --> APIGW
    APIGW -- Response --> ClientUI
    ProjectionSvc -- Push/Notify --> ClientUI
```

### Key Features
 * Strict separation of write (commands/events) and read (queries/projections).
 * Event store is the source of truth.
 * Event bus decouples event propagation.
 * Projection service updates read models and can push updates (e.g., via WebSockets) to UI.
 * Idempotency and replay are built-in.

|- Pros -|- Cons -|
| Full audit/history, replayability, scalable reads, real-time updates | Complex, requires careful design, eventual consistency, debugging can be harder |