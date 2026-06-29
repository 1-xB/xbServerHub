import { useEffect, useState } from "react";
import { getConnection } from "../services/signalService";


export interface CpuMetrics {
    usagePercent: number;
    userPercent: number;
    systemPercent: number;
}

export interface MemoryMetrics {
    totalBytes: number;
    availableBytes: number;
    usedBytes: number;
    usedPercent: number;
}

export interface NetworkMetrics {
    interfaceName: string;
    downloadSpeed: number; 
    downloadTotal: number;
    uploadSpeed: number; 
    uploadTotal: number;
}

export interface ServerMetrics {
    cpu: CpuMetrics;
    memory: MemoryMetrics;
    networkInterfaces: NetworkMetrics[];
    fetchedAtUtc: string;
}

export function useMetricsHub() {
    const [metrics, setMetrics] = useState<ServerMetrics | null>(null);

    useEffect(() => {
        const connection = getConnection();

        const handleReceiveMetrics = (updatedMetrics: ServerMetrics) => {
            setMetrics(updatedMetrics);
        }

        connection.on("ReceiveMetrics", handleReceiveMetrics);
        return () => {
            connection.off("ReceiveMetrics", handleReceiveMetrics);
        }
    }, [])

    return metrics;
}