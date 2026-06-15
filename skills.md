This structure works well for enterprise-scale contract testing because the same skill can be deployed across regional API gateways while keeping region-specific provider endpoints and A2A routing.

# Pact Contract Governance Skill

## Region Configuration

### America

region_code: AMER
provider_gateway: [https://api-amer.company.com](https://api-amer.company.com)
broker_url: [https://pact-broker-amer.company.com](https://pact-broker-amer.company.com)

### Europe

region_code: EMEA
provider_gateway: [https://api-eu.company.com](https://api-eu.company.com)
broker_url: [https://pact-broker-eu.company.com](https://pact-broker-eu.company.com)

### Asia

region_code: APAC
provider_gateway: [https://api-asia.company.com](https://api-asia.company.com)
broker_url: [https://pact-broker-asia.company.com](https://pact-broker-asia.company.com)

### Australia

region_code: AUS
provider_gateway: [https://api-aus.company.com](https://api-aus.company.com)
broker_url: [https://pact-broker-aus.company.com](https://pact-broker-aus.company.com)

---

# Purpose

Validate that API contract changes are safe for deployment across all supported regions.

The skill performs:

* Consumer contract generation
* Provider verification
* Schema validation
* Backward compatibility validation
* Bidirectional contract validation
* Pact publication
* Can-I-Deploy verification
* Cross-agent collaboration through A2A connections

---

# When To Use

Use this skill when:

* Consumer contracts change
* Provider implementations change
* OpenAPI specifications change
* New API versions are released
* Deployment approvals are required
* Regional API gateways are updated
* A user asks whether a contract change is safe

---

# Required Input Parameters

| Parameter        | Required | Description                |
| ---------------- | -------- | -------------------------- |
| region           | Yes      | AMER, EMEA, APAC, AUS      |
| contractType     | Yes      | consumer/provider          |
| consumerName     | Yes      | Consumer application       |
| providerName     | Yes      | Provider API               |
| consumerVersion  | Yes      | Consumer version           |
| providerVersion  | Yes      | Provider version           |
| environment      | Yes      | dev/test/stage/prod        |
| pactFile         | Optional | Pact contract path         |
| openApiSpec      | Optional | OpenAPI specification path |
| deploymentTarget | Optional | Target environment         |

---

# Policy

1. Provider deployment must never proceed without successful verification.
2. Consumer contracts must be generated from executable tests.
3. Schema validation alone cannot determine deployment approval.
4. Can-I-Deploy decision is mandatory.
5. Every regional deployment must be verified independently.
6. Contract failures automatically block deployment.
7. All verification evidence must be retained.

---

# Constraints

* Provider verification timeout: 10 minutes
* Consumer test timeout: 10 minutes
* Regional verification executed independently
* No deployment approval if any region fails
* Contract files must be generated within current pipeline execution
* OpenAPI specification must match deployed version

---

# Workflow Steps

Step 1
Identify region.

Step 2
Determine contract type.

Step 3
Execute consumer contract tests.

Step 4
Generate pact contracts.

Step 5
Validate OpenAPI schema.

Step 6
Perform backward compatibility validation.

Step 7
Perform bidirectional contract verification.

Step 8
Verify provider against generated contracts.

Step 9
Publish contracts to Broker/PactFlow.

Step 10
Execute Can-I-Deploy verification.

Step 11
Generate deployment decision.

Step 12
Produce audit report.

---

# Tools

## MCP Tool: run_consumer_tests

Purpose:
Generate consumer contracts.

Input:
{
"consumerName": "",
"region": ""
}

Execution:
dotnet test Consumer.Tests

Output:
Generated pact files.

---

## MCP Tool: generate_pacts

Purpose:
Create pact contracts from consumer tests.

Input:
{
"consumerName": "",
"region": ""
}

Output:
Pact file locations.

---

## MCP Tool: validate_openapi_schema

Purpose:
Validate requests and responses against OpenAPI specification.

Input:
{
"specFile": ""
}

Output:
Validation status.

---

## MCP Tool: compare_openapi_versions

Purpose:
Detect backward compatibility violations.

Input:
{
"oldVersion": "",
"newVersion": ""
}

Output:
Breaking change report.

---

## MCP Tool: verify_provider

Purpose:
Verify provider implementation against contracts.

Input:
{
"providerName": "",
"region": ""
}

Execution:
dotnet test Provider.Tests

Output:
Verification status.

---

## MCP Tool: publish_pacts

Purpose:
Publish contracts.

Input:
{
"region": "",
"consumerVersion": ""
}

Output:
Publication status.

---

## MCP Tool: can_i_deploy

Purpose:
Determine deployment safety.

Input:
{
"consumerVersion": "",
"providerVersion": ""
}

Output:
Deployment decision.

---

# A2A Agent Connections

## Consumer Agent

Responsibilities:

* Generate consumer contracts
* Publish consumer contracts
* Request provider validation

Connected Provider Agent:

provider-contract-agent

Communication:

A2A Message:

{
"action":"verify_contract",
"provider":"AddressBook.API",
"contract":"consumer1-addressbook.api.json"
}

---

## Provider Agent

Responsibilities:

* Verify consumer contracts
* Validate implementation
* Publish verification results

Connected Consumer Agent:

consumer-contract-agent

Communication:

A2A Message:

{
"action":"verification_result",
"status":"PASSED",
"provider":"AddressBook.API"
}

---

# Output Format

## Deployment Decision JSON

{
"region":"AMER",
"consumer":"Consumer1",
"provider":"AddressBook.API",
"consumerVersion":"1.2.0",
"providerVersion":"2.0.0",
"schemaValidation":"PASSED",
"backwardCompatibility":"PASSED",
"bidirectionalVerification":"PASSED",
"providerVerification":"PASSED",
"canIDeploy":true,
"decision":"SAFE_TO_DEPLOY",
"timestamp":"2026-06-14T00:00:00Z"
}

---

# Example JSON Input

{
"region":"EMEA",
"contractType":"provider",
"consumerName":"CustomerPortal",
"providerName":"AddressBook.API",
"consumerVersion":"1.5.0",
"providerVersion":"2.1.0",
"environment":"stage",
"openApiSpec":"./swagger/v2/openapi.yaml",
"pactFile":"./pacts/customerportal-addressbook.json"
}

---

# Exceptions

## Missing Pact File

Return:

{
"status":"FAILED",
"reason":"PACT_FILE_NOT_FOUND"
}

---

## Provider Unreachable

Return:

{
"status":"FAILED",
"reason":"PROVIDER_UNAVAILABLE"
}

---

## Schema Validation Failure

Return:

{
"status":"FAILED",
"reason":"SCHEMA_VALIDATION_FAILED"
}

---

## Contract Verification Failure

Return:

{
"status":"FAILED",
"reason":"PACT_VERIFICATION_FAILED"
}

---

# verify_provider

Execution Sequence:

1. Load pact contracts.
2. Load provider state.
3. Execute verification.
4. Compare expected interactions.
5. Validate response schema.
6. Publish results.
7. Generate verification report.

Verification Rules:

* Status code must match.
* Required fields must exist.
* Data types must match.
* Contract expectations must be satisfied.
* No breaking response changes allowed.

---

# Verification Procedure

1. Execute run_consumer_tests.
2. Generate pact contracts.
3. Validate schema.
4. Compare OpenAPI versions.
5. Execute bidirectional verification.
6. Execute verify_provider.
7. Publish contracts.
8. Execute can_i_deploy.
9. Produce deployment report.

---

# Failure Analysis

For every failure:

Capture:

* Interaction ID
* Consumer name
* Provider name
* Region
* Expected payload
* Actual payload
* Missing fields
* Renamed fields
* Status code mismatch
* Schema violations
* Backward compatibility violations

Classification:

Severity 1
Deployment blocked.

Severity 2
Manual review required.

Severity 3
Warning only.

---

# Success Criteria

Deployment approval is granted only when:

✓ Consumer tests pass

✓ Pact contracts generated

✓ OpenAPI validation passes

✓ Backward compatibility validation passes

✓ Bidirectional verification passes

✓ Provider verification passes

✓ Contracts published successfully

✓ Can-I-Deploy returns TRUE

✓ No regional failures exist

Final Result:

{
"decision":"SAFE_TO_DEPLOY",
"canIDeploy":true
}

This template is suitable for a multi-region enterprise setup and can be used directly as a reusable `skill.md` for MCP-driven Pact verification, OpenAPI compatibility checks, and A2A consumer-provider agent collaboration.
