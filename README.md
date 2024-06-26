# AnalyticsPipeline

## Summary

As organizations increasingly rely on data-driven insights, creating a robust analytics system becomes paramount.
My project aims to deliver an opinionated approach to building such a system, combining efficiency, scalability, and practicality.
Let’s dive into the key aspects:

### Objective and Scope:

- Analytics system serves as a bridge between raw data and actionable insights. It empowers decision-makers by providing real-time and historical data analysis.
- That system focus on a medium-scale solution, striking a balance between agility and depth.

### Components:

- **Client-Side SDK**: our SDK facilitates seamless data collection from various sources (Unity game engine: standalone, web, mobile apps). It ensures minimal impact on user experience while capturing essential events.
- **Operative and Historical Data Storage:**
  - **Operative Data:** real-time data storage for immediate processing. Minimal latency, but limited volume / time range.
  - **Historical Data:** long-term storage for trend analysis, reporting, and compliance. Enables retrospective insights and predictive modeling. Full data retention, but higher latency.
  - **Data mart** - both data storages allows to query data using SQL and set up useful views for business users.

### Technology Stack:

I’ve chosen the .NET ecosystem for several reasons:
- **Unity Game Developers:** .NET is familiar to Unity developers, allowing seamless transitions between game development and analytics.
- **Language Consistency:** Using C# for both client and server-side development streamlines code maintenance and collaboration.
- **Rich Libraries:** Leverage .NET libraries for data manipulation, security, and scalability.

### Business Impact:

That analytics system empowers stakeholders to:
- Optimize user experiences based on real-time data.
- Identify bottlenecks, anomalies, and growth opportunities.
- Make informed decisions backed by historical trends.
- Enhance operational efficiency and resource allocation.

In summary, my opinionated analytics system combines technical excellence with practicality, enabling data-driven success.
Let’s turn insights into action! 🚀📊

## Architecture chart legend

```mermaid
flowchart TD
ICA(Internal Component A: tech descripotion) --> |Sync operation| EC>External Component]
EC -.-> |Async operation| ICB(Internal Component B: tech description)
ICB ==> |Sync write operation| S[(Storage)]
```

## Abstract architecture

```mermaid
flowchart TD
    C(Client SDK) --> |HTTP POST| R(Data Receiver)
    R --> Q>Event Queue]
    Q -.-> |Poll| W(Data Writer)
    W ==> OD[(Operative Database)]
    OD -.-> |Retention period| M(Data Migrator)
    M ==> HD[(Historical Database)]
    OD --> DM>Data Mart]
    HD --> DM
```

## Self-hosted architecture

```mermaid
flowchart TD
C(Client SDK: C#, Unity) --> |HTTP POST| LB
subgraph docker compose
LB>nginx]
R(Data Receiver: C#, ASP.NET Core)
Q>redpanda]
W(Data Writer: C#, ASP.NET Core, EF.Core)
M(Data Migrator: C#, ASP.NET Core, EF.Core)
P>Apache Presto]
DM>Data Mart: Apache Superset]
end
LB --> R
R --> Q
Q -.-> |Poll| W
subgraph Local Database
OD[(Operative Database: Postgres)]
end
W ==> OD
OD -.-> |Retention period| M
subgraph Local MiniIO
HD[(Historical database bucket)] --> P
end
M ==> HD
OD --> DM
P --> DM
```

## Cloud architecture (AWS-based)

```mermaid
flowchart TD
C(Client SDK: C#, Unity) --> |HTTP POST| ALB>Load balancer: AWS ALB]
ALB --> R 
subgraph ECS [AWS ECS]
R(Data Receiver: C#, ASP.NET Core)
W(Data Writer: C#, ASP.NET Core, EF.Core)
M(Data Migrator: C#, ASP.NET Core, EF.Core)
P>Apache Presto]
DM>Data Mart: Apache Superset]
end
R --> Q>Event Queue: AWS SQS]
Q -.-> |Poll| W
subgraph RDS [AWS RDS]
OD[(Operative Database: Postgres)]
end
W ==> OD
OD -.-> |Retention period| M
subgraph S3 [AWS S3]
HD[(Historical database bucket)] --> P
end
M ==> HD
OD --> DM
P --> DM
```

## Self-hosted implementation

### Dependencies

- Docker

### Usage

To run whole system:

```
docker-compose up -d
```

To run with full services rebuild:

``` 
docker-compose up -d --build
```

#### Superset configuration

To setup Superset, you need to run the following commands:

```
docker exec -it analyticspipeline-superset sh
export FLASK_APP=superset; flask fab create-admin
superset db upgrade
superset init
```

Then you can access Superset at `http://localhost:8088`.

To configure access to operative storage, you need to add a new database connection using web UI:

```
Plus Sign -> Data -> Connect database:
> Host: postgres
> Post: 5432
> Database name: analytics_database
> Username: analytics_r_user
> Password: analytics_r_user_password
```

### Examples

#### Unity Sample Project

![unity sample project](./static/client_prototype.png)

#### Superset Dashboard

![superset dashboard 1](./static/superset_dashboard_1.png)