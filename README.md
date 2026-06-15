# MS-Hackathon-2026

B2B Agentic Contract testing
Problem : Legacy contract based testing using pact flow or pact broker involves manual intervention of verifying Can I deploy? based on outcome of test method executing success or failure. This leads to back and forth involving enormous time spend by both provider and consumer. The disagreement often leads to wastage of time involving back and forth.

Solution : Leverage Agentic AI using GitHub copilot, Microsoft foundry and Microsoft office 365 to maintain skills which will include policies and well guided steps for both provider and consumer. AI Agents from Consumer environment and provider environment confirm "Can-AI-deploy" instead of traditional "can-i-deploy" flow.

AI Agents take over contract testing, validations and deployments with detailed reports and contract testing parameters persisted for future use.

Skills will correspond to various contract testing strategies as follows :

Provider driven
Consumer driven
Bidirectional
Schema validation testing
Backward compatibility testing
Integrated sandbox testing
Snapshot/goldmaster testing
Synthetic consumer testing

The project can be further scaled for handling alternatives to pact flow and add scalability to choose between vendors for overall SaaS cost savings.

Consumer Agent
│
▼
MCP Server
(run tests + verify contracts)
│
▼
Provider Agent
│
▼
Gatekeeper Agent
(Can-I-Deploy Decision)
│
▼
Deployment Agent
(Azure DevOps / GitHub Actions)

PR Created
│
▼
Consumer Agent
│
▼
run_consumer_tests
│
▼
generate_pacts
│
▼
Provider Agent
│
▼
verify_provider
│
▼
Gatekeeper Agent
│
▼
can_ai_deploy
│
├── FALSE → Block Deployment
│
└── TRUE
│
▼
Deployment Agent
│
▼
Azure DevOps Release

This pattern gives you a clean separation:

Consumer AI Agent = generates and validates contracts.
Provider AI Agent = verifies implementation against contracts.
Gatekeeper AI Agent = the single authority that reads can_i_deploy.
Deployment AI Agent = deploys only when the gatekeeper returns SAFE_TO_DEPLOY.

Further scope for this project : Add scalability to switch between pactflow and pact broker.

For scalability in future, remove the hard-coded PactFlow or Pact Broker anywhere in the agents. Instead, we will introduce a Contract Platform Abstraction Layer. Sample code added. To do: refactor and add security layer as per organizational policies. Update policy in skills.md which will ensure microsoft purview, or even rayfin to do prompt to deploy using strict governance and compliance as defined by standard procedures and policies.

Consumer Agent
│
▼
ContractVerificationAgent
│
▼
IContractPlatform
│
┌────┴──────────────┐
▼ ▼
PactBrokerAdapter PactFlowAdapter
│ │
▼ ▼
Broker API PactFlow API

This allows:
 -> Self-hosted Pact Broker today 
 -> PactFlow tomorrow
 -> Regional brokers later
 -> Zero changes to AI agents

Recommended Enterprise Extension:
IContractPlatform
├── PactBrokerAdapter
├── PactFlowAdapter
└── RegionalContractAdapter

The RegionalContractAdapter routes automatically:
AMER → PactFlow
EMEA → Pact Broker
APAC → PactFlow
AUS → PactFlow

Based on region from the skill input.

This allows gradual migration from Pact Broker to PactFlow region-by-region while keeping the AI Consumer Agent, AI Provider Agent, Gatekeeper Agent, MCP tools, and Deployment Agent unchanged.

Consumer Agent
│
▼
MCP Verification Agent
│
├─ run_consumer_tests
├─ validate_openapi_schema
├─ compare_openapi_versions
├─ verify_provider
└─ can_i_deploy
│
▼
Gatekeeper A2A Agent
│
├─ APPROVED
│ ▼
│ Deployment Agent
│
└─ REJECTED
▼
Consumer Agent
Provider Agent

PactFlow supports everything Pact Broker does plus:
1) Bi-directional contracts
2) Environment management
3) Branch support
4) Webhooks
5) Deployment records

Further enhancements required to gatekeeper AI agent on both consumer and provider side to record audit trail for observability using Azure monitor:
1) Who requested deployment
2) Consumer version
3) Provider version
4) Decision
5) Timestamp
6) Region

Microsoft foundry integration notes: 
Go to your agent definition in the Foundry Portal.Navigate to Builder > Knowledge base.Click Add repository.Select GitHub or Azure DevOps.Sign in, authorize access, and select the specific repository from the dropdown menu.The platform will automatically parse, chunk, vectorize, and index the code into a managed vector store for the agent's semantic search.

Sample repo which Microsoft foundry agent : https://github.com/jayeshtoscano/contract-testing-dotnetcore-example

Microsoft 365 Copilot can act as the collaboration + orchestration layer that sits above your MCP + A2A + Pact system. It doesn’t replace your agents—it makes them discoverable, conversational, and workflow-driven inside the Microsoft 365 ecosystem.

Role description : 
MCP is execution layer
A2A agents are decision/execution layer
Microsoft 365 Copilot is nothing but human + enterprise collaboration layer that drives and supervises A2A.

Microsoft 365 Copilot's role : Consumer Agent → MCP → Provider Agent → Gatekeeper → Deployment

Copilot becomes the natural language control plane.

User (Teams / Outlook / Word)
        ↓
Microsoft 365 Copilot
        ↓
Orchestrates A2A requests
        ↓
Consumer Agent ↔ Provider Agent
        ↓
MCP (tests + contracts)
        ↓
Gatekeeper decision
        ↓
Back to Copilot (explanation + approval summary)


Key roles Copilot plays in A2A is 
a) Natural language trigger for A2A workflows. i.e. Instead of calling MCP manually, Run contract validation for AddressBook API.
b) Cross-team communication bridge (Consumer ↔ Provider)

           Without Copilot:
           Consumer team opens Jira ticket
           Provider team checks logs
           Slack/Email back-and-forth
           
           With Copilot:
           
           In Teams chat: Does AddressBook API change break CustomerPortal?
           Copilot:
           > Calls Consumer Agent
           > Calls Provider Agent
           > Runs MCP verification
           > Returns consolidated answer
           
           Example response:
           Breaking change detected
           Field name → fullName
           2 consumers impacted
           Deployment blocked

C) Automated A2A coordination (no manual routing)

Copilot can act as a router:

Trigger	Copilot Action
PR created	call Consumer Agent
API changed	call Provider Agent
OpenAPI updated	run compatibility MCP tool
Deployment requested	invoke Gatekeeper Agent

D) “Explainability layer” for Gatekeeper decisions
   Instead of raw JSON: {  "canAIDeploy": false   } , gives details of issue like for example Provider removed required field name, Breaks CustomerPortal contract v1.5, 3 failing pact interactions detected, etc.

Why Copilot is important in your architecture ?
Without Copilot:
A2A is system-driven only
Developers must use APIs/CLI
Contract insights are fragmented

Also , Approval workflows (human-in-the-loop A2A)


Copilot connects to MCP + A2A using Copilot Studio or use Graph Connectors.
