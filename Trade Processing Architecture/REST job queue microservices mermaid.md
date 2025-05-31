```mermaid
flowchart TD
    UI2[Trade UI]
    APIGW2[API Gateway]
    TradeAPI2[Trade Service]
    Queue2[[Job Queue]]
    Booking2[Booking Worker]
    DB2[(Database)]
    Notify2[Notification Service]

    UI2 --> APIGW2
    APIGW2 --> TradeAPI2
    TradeAPI2 --> Queue2
    Queue2 --> Booking2
    Booking2 --> DB2
    Booking2 --> TradeAPI2
    TradeAPI2 --> DB2
    TradeAPI2 --> Notify2
    Notify2 --> UI2
    TradeAPI2 -. Roll Forwards/Backwards .-> DB2
```