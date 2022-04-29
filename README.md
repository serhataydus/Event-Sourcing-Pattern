# Event-Sourcing-Pattern
.Net Core Event Sourcing Pattern

DockerFile üzerinden Event Store ayağa kaldırmak için kullanılacak kodlar

docker build -t eventstore/eventstore:with-cert-local --no-cache .

docker run --name eventstore-node -it -p 1113:1113 -p 1115:1115 -p 2113:2113 -e EVENTSTORE_CERTIFICATE_FILE=eventstore.p12 -e EVENTSTORE_EXT_SECURE_TCP_PORT=1115 eventstore/eventstore:with-cert-local
