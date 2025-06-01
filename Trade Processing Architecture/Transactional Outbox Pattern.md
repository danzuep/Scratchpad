## Transactional Outbox Pattern

> Event-based messaging with Command Query Responsibility Segregation.

```mermaid
flowchart TD
    subgraph External
        ClientUI
        APIGW["API Gateway"]
    end

    subgraph Internal
        Svc["Service"]
        Outbox[("Outbox")]
        Broker[["Event Bus"]]
        Worker["Worker(s)"]
    end

    ClientUI -- Request --> APIGW
    APIGW -- Command --> Svc
    Svc -- Event Created --> Outbox
    Svc -- Publish --> Broker
    Broker -- Subscribe --> Worker
    Worker -- Event Processed --> Outbox
    Outbox -- Query --> APIGW
    APIGW -- Response --> ClientUI
```

### Key Features
 * Trade service writes event to an outbox table within the same transaction as the business entity.
 * Outbox table is polled, and events are published to the broker.
 * Reliable, avoids dual-write issues.
 * Simpler than full event sourcing, but no full event history.

|- Pros -|- Cons -|
| Simpler, transactional integrity, easier to retrofit onto existing systems | No full event sourcing, no event replay, outbox polling adds latency |