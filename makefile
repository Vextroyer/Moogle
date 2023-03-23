.PHONY: build
build:
	dotnet build

.PHONY: dev
dev:
	dotnet run --project MoogleServer
