import React from 'react';
import { Navigate } from 'react-router-dom';

interface ProtectedRouteProps {
    isLoggedIn: boolean;
    children: React.ReactNode;
}

function ProtectedRoute({isLoggedIn, children}: ProtectedRouteProps) {
    if (!isLoggedIn) {
        return <Navigate to="/auth" replace />;
    }
    return <>{children}</>
}

export default ProtectedRoute;