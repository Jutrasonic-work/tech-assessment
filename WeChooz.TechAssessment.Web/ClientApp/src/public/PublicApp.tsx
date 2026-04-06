import { BrowserRouter, Navigate, Route, Routes } from "react-router-dom";
import { AppProviders } from "@/shared/theme/AppProviders";
import { PublicLayout } from "@/public/layout/PublicLayout";
import { CatalogPage } from "@/public/pages/CatalogPage";
import { SessionDetailPage } from "@/public/pages/SessionDetailPage";

export function PublicApp() {
    return (
        <AppProviders>
            <BrowserRouter>
                <Routes>
                    <Route element={<PublicLayout />}>
                        <Route index element={<CatalogPage />} />
                        <Route path="session/:sessionId" element={<SessionDetailPage />} />
                        <Route path="*" element={<Navigate to="/" replace />} />
                    </Route>
                </Routes>
            </BrowserRouter>
        </AppProviders>
    );
}
