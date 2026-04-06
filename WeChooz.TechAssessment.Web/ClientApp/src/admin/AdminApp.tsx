import { BrowserRouter, Navigate, Route, Routes } from "react-router-dom";
import { AppProviders } from "@/shared/theme/AppProviders";
import { AdminChrome } from "@/admin/layout/AdminChrome";
import { RequireLogin } from "@/admin/guards/RequireLogin";
import { FormationOnly } from "@/admin/guards/FormationOnly";
import { LoginPage } from "@/admin/pages/LoginPage";
import { AdminStart } from "@/admin/pages/AdminStart";
import { SalesSessionAccessPage } from "@/admin/pages/SalesSessionAccessPage";
import { CoursesListPage } from "@/admin/pages/CoursesListPage";
import { CourseEditPage } from "@/admin/pages/CourseEditPage";
import { SessionsListPage } from "@/admin/pages/SessionsListPage";
import { SessionEditPage } from "@/admin/pages/SessionEditPage";
import { AdminSessionDetailPage } from "@/admin/pages/AdminSessionDetailPage";
import { SessionParticipantsPage } from "@/admin/pages/SessionParticipantsPage";

export function AdminApp() {
    return (
        <AppProviders>
            <BrowserRouter basename="/admin">
                <Routes>
                    <Route path="login" element={<LoginPage />} />
                    <Route element={<RequireLogin />}>
                        <Route element={<AdminChrome />}>
                            <Route index element={<AdminStart />} />
                            <Route path="sales-access" element={<SalesSessionAccessPage />} />
                            <Route element={<FormationOnly />}>
                                <Route path="courses" element={<CoursesListPage />} />
                                <Route path="courses/new" element={<CourseEditPage />} />
                                <Route path="courses/:courseId/edit" element={<CourseEditPage />} />
                                <Route path="sessions" element={<SessionsListPage />} />
                                <Route path="sessions/new" element={<SessionEditPage />} />
                                <Route path="sessions/:sessionId/edit" element={<SessionEditPage />} />
                                <Route path="sessions/:sessionId" element={<AdminSessionDetailPage />} />
                            </Route>
                            <Route path="sessions/:sessionId/participants" element={<SessionParticipantsPage />} />
                        </Route>
                    </Route>
                    <Route path="*" element={<Navigate to="/" replace />} />
                </Routes>
            </BrowserRouter>
        </AppProviders>
    );
}
