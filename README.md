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
