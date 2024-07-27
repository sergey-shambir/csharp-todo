#!/usr/bin/env bash

set -o errexit

readonly MSSQL_SA_PASSWORD=Buquaem6
readonly SCRIPTS_DIR=/App/scripts

echo_call() {
    echo "$@"
    "$@"
}

wait_tcp_port_open() {
    local PORT=$1
    local TIMEOUT=$2
    echo_call timeout $TIMEOUT bash -c 'until (echo > /dev/tcp/127.0.0.1/$0) >/dev/null 2>&1; do sleep 1; done' "$PORT"
    echo "Port $PORT is open for TCP connections"
}

execute_sql_script() {
    local SQL_SCRIPT_NAME=$1
    echo_call /opt/mssql-tools/bin/sqlcmd -r -b -S localhost -U sa -P "$MSSQL_SA_PASSWORD" -i "$SCRIPTS_DIR/$SQL_SCRIPT_NAME"
}

wait_tcp_port_open 1433 30s
execute_sql_script create-todo-db-and-user.sql
execute_sql_script init-todo-schema.sql
echo "Initialized OK"
