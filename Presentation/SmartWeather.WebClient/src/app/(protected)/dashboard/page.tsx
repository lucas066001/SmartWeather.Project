import MainLayout from "@/components/ui/MainLayout";
import SocketConnector from "@/components/ui/socket/socketConnector";

function DashboardPage() {
  return (
    <MainLayout title="Dashboard">
      <SocketConnector />
    </MainLayout>
  );
}

export default DashboardPage